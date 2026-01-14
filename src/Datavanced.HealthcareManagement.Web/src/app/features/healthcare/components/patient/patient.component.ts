import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Patient, CreatePatientDto } from '../../models/patient.model';
import { Caregiver } from '../../models/caregiver.model';
import { PatientService } from '../../services/patient.service';
import { CaregiverService } from '../../services/caregiver.service';
import { Subject, debounceTime, distinctUntilChanged } from 'rxjs';
import { PaginationRequest, ApiResponse } from '../../../../core/models/core.model';

@Component({
  selector: 'app-patient',
  standalone: false,
  templateUrl: './patient.component.html'
})
export class PatientComponent implements OnInit {
  patients: Patient[] = [];
  allCaregivers: Caregiver[] = [];
  totalCount = 0;

  request: PaginationRequest = { pageIndex: 1, pageSize: 10, sortBy: 'FirstName', descending: false, keyword: '' };
  searchSubject = new Subject<string>();
  patientForm!: FormGroup;
  showForm = false;
  isEditing = false;

  constructor(private fb: FormBuilder, private patientService: PatientService, private caregiverService: CaregiverService
  ) {
    this.initForm();
  }

  ngOnInit(): void {
    this.loadCaregivers();
    this.loadPatients();

    this.searchSubject.pipe(debounceTime(400), distinctUntilChanged()).subscribe(val => {
      this.request.keyword = val;
      this.request.pageIndex = 1;
      this.loadPatients();
    });
  }

  initForm() {
    this.patientForm = this.fb.group({
      patientId: [0],
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      phone: ['', Validators.required],
      dateOfBirth: ['', Validators.required],
      officeId: [1],
      caregiverIds: [[]]
    });
  }

  loadCaregivers() {
    this.caregiverService.getAllCaregivers().subscribe(res => {
      if (!res.isError) this.allCaregivers = res.result;
    });
  }

  loadPatients() {
    this.patientService.getPatients(this.request).subscribe(res => {
      if (!res.isError) {
        this.patients = res.result.data;
        this.totalCount = res.result.totalCount;
      }
    });
  }

  onSort(column: string) {
    if (this.request.sortBy === column) {
      this.request.descending = !this.request.descending;
    } else {
      this.request.sortBy = column;
      this.request.descending = false;
    }
    this.request.pageIndex = 1; 
    this.loadPatients();
  }

  editPatient(patient: Patient) {
    this.isEditing = true;
    this.showForm = true;
    const dob = patient.dateOfBirth ? patient.dateOfBirth.split('T')[0] : '';
    this.patientForm.patchValue({
      ...patient,
      dateOfBirth: dob,
      caregiverIds: patient.caregivers.map(c => c.caregiverId)
    });
  }

  onSubmit() {
    if (this.patientForm.invalid) return;
    const formValue = this.patientForm.value;

    const patientId = formValue.patientId;

    const patientDto: CreatePatientDto = {
      officeId: formValue.officeId,
      firstName: formValue.firstName,
      lastName: formValue.lastName,
      dateOfBirth: formValue.dateOfBirth,
      phone: formValue.phone,
      email: formValue.email,
      isActive: formValue.isActive ?? true,
      caregivers: formValue.caregiverIds 
    };

    if (this.isEditing && patientId > 0) {
      this.patientService.updatePatient(patientId, patientDto).subscribe({
        next: (res) => this.handleResponse(res),
        error: (err) => console.error('Update failed', err)
      });
    } else {
      this.patientService.createPatient(patientDto).subscribe({
        next: (res) => this.handleResponse(res),
        error: (err) => console.error('Creation failed', err)
      });
    }
  }

  private handleResponse(res: ApiResponse<any>) {
    if (!res.isError) {
      this.cancel(); 
      this.loadPatients(); 
    } else {
      alert(res.errorMessage);
    }
  }

  deletePatient(id: number) {
    // Simple browser confirmation (or use a custom modal)
    if (confirm('Are you sure you want to delete this patient record? This action cannot be undone.')) {
      this.patientService.deletePatient(id).subscribe({
        next: (res) => {
          if (!res.isError) {
            // Refresh the list and show success
            this.loadPatients();
          } else {
            alert(res.errorMessage || 'An error occurred while deleting.');
          }
        },
        error: (err) => {
          console.error('Delete failed', err);
          alert('Server error: Could not delete patient.');
        }
      });
    }
  }

  cancel() {
    this.showForm = false;
    this.isEditing = false;
    this.patientForm.reset({ patientId: 0, officeId: 1, caregiverIds: [] });
  }
}
