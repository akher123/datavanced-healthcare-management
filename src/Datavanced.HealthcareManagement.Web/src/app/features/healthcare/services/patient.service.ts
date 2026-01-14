import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable, map } from 'rxjs';
import { Patient } from '../models/patient.model';
import { ApiResponse, PagedResult, PaginationRequest } from '../../../core/models/core.model';

@Injectable({ providedIn: 'root' })
export class PatientService {

  private apiUrl = 'https://localhost:7024/api/hcms/patients';

  constructor(private http: HttpClient) { }

  getPatients(req: PaginationRequest): Observable<ApiResponse<PagedResult<Patient>>> {
    let params = new HttpParams()
      .set('PageIndex', req.pageIndex.toString())
      .set('PageSize', req.pageSize.toString())
      .set('SortBy', req.sortBy || 'PatientId')
      .set('Descending', req.descending.toString());

    if (req.keyword) {
      params = params.set('Keyword', req.keyword);
    }

    return this.http.get<ApiResponse<PagedResult<Patient>>>(
      `${this.apiUrl}/get-patients-by-Pagination`,
      { params }
    );
  }


  createPatient(patient: any): Observable<ApiResponse<any>> {
    return this.http.post<ApiResponse<any>>(`${this.apiUrl}/create-patient`, patient);
  }


  updatePatient(id: number, patient: any): Observable<ApiResponse<any>> {
    return this.http.put<ApiResponse<any>>(`${this.apiUrl}/update-patient/${id}`, patient);
  }

  deletePatient(id: number): Observable<ApiResponse<any>> {
    return this.http.delete<ApiResponse<any>>(
      `${this.apiUrl}/delete-patient-by-id/${id}`
    );
  }


}
