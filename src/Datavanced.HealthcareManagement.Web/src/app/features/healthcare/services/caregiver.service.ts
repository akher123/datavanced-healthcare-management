import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, map } from 'rxjs';
import { Caregiver } from '../models/caregiver.model';
import { ApiResponse, PagedResult, PaginationRequest } from '../../../core/models/core.model';
import { API_BASE_URL, API_ENDPOINTS } from '../../../core/config/api.config';
@Injectable({
  providedIn: 'root'
})
export class CaregiverService {

  private readonly apiUrl: string;
  constructor(private http: HttpClient,
    @Inject(API_BASE_URL) baseUrl: string) {
    this.apiUrl = `${baseUrl}${API_ENDPOINTS.caregivers}`;
  }

  getAllCaregivers(): Observable<ApiResponse<Caregiver[]>> {
    return this.http.get<ApiResponse<Caregiver[]>>(this.apiUrl + "/get-caregivers").pipe(
      map(res => {
        if (!res.isError && res.result) {
          return res;
        }

        return {
          ...res,
          result: []
        };
      })
    );
  }

}
