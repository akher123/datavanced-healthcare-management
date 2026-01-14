import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, map } from 'rxjs';
import { Caregiver } from '../models/caregiver.model';
import { ApiResponse, PagedResult, PaginationRequest } from '../../../core/models/core.model';

@Injectable({
  providedIn: 'root'
})
export class CaregiverService {

  private apiUrl = 'https://localhost:7024/api/hcms/caregivers';
  constructor(private http: HttpClient) { }

  getAllCaregivers(): Observable<ApiResponse<Caregiver[]>> {
    return this.http.get<ApiResponse<Caregiver[]>>(this.apiUrl +"/get-caregivers").pipe(
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
