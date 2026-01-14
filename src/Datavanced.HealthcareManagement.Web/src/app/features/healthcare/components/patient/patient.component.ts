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
  templateUrl: './patient.component.html',
  styles: [`
    .x-small { font-size: 0.75rem; }
    .btn-xs { font-size: 0.75rem; padding: 0.25rem 0.5rem; }
    .cursor-pointer { cursor: pointer; }
    .form-label { margin-bottom: 0.2rem; font-weight: 600; color: #495057; }
  `]
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

  constructor(private fb: FormBuilder, private patientService: PatientService, private caregiverService: CaregiverService) {
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
      firstName: ['', [Validators.required, Validators.minLength(2)]],
      lastName: ['', [Validators.required, Validators.minLength(2)]],
      email: ['', [Validators.required, Validators.email]],
      phone: ['', [Validators.required, Validators.pattern('^[0-9-+ ]*$')]],
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
    if (this.patientForm.invalid) {
      this.patientForm.markAllAsTouched();
      return;
    }

    const formValue = this.patientForm.value;
    const patientDto: CreatePatientDto = {
      officeId: formValue.officeId,
      firstName: formValue.firstName,
      lastName: formValue.lastName,
      dateOfBirth: formValue.dateOfBirth,
      phone: formValue.phone,
      email: formValue.email,
      isActive: true,
      caregivers: formValue.caregiverIds
    };

    if (this.isEditing && formValue.patientId > 0) {
      this.patientService.updatePatient(formValue.patientId, patientDto).subscribe({
        next: (res) => this.handleResponse(res),
        error: (err) => alert('Update failed')
      });
    } else {
      this.patientService.createPatient(patientDto).subscribe({
        next: (res) => this.handleResponse(res),
        error: (err) => alert('Creation failed')
      });
    }
  }

  deletePatient(id: number) {
    if (confirm('Permanently delete this patient and their caregiver assignments?')) {
      this.patientService.deletePatient(id).subscribe({
        next: (res) => {
          if (!res.isError) this.loadPatients();
          else alert(res.errorMessage);
        }
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

  cancel() {
    this.showForm = false;
    this.isEditing = false;
    this.patientForm.reset({ patientId: 0, officeId: 1, caregiverIds: [] });
  }

  // Helper for Template
  isInvalid(controlName: string): boolean {
    const control = this.patientForm.get(controlName);
    return !!(control && control.invalid && (control.dirty || control.touched));
  }
}
