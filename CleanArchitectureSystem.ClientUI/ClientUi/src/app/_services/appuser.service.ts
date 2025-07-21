import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environment/environment.dev';
import { Observable } from 'rxjs';
import { RegisterUserRequest } from '../_models/appuser/registerUserRequest';
import { CustomResultResponse } from '../_models/shared/customResultResponse';
import { UpdateUserRequest } from '../_models/appuser/updateUserRequest';
import { ViewUserRequest } from '../_models/appuser/viewUserRequest';
import { ResetUserRequest } from '../_models/appuser/resetUserRequest';

@Injectable({
  providedIn: 'root',
})
export class AppuserService {
  private readonly baseUrl = `${environment.apiUrl}userauth`;

  constructor(private http: HttpClient) {}

  getAll(): Observable<ViewUserRequest[]> {
    return this.http.get<ViewUserRequest[]>(this.baseUrl);
  }

  save(payload: RegisterUserRequest): Observable<CustomResultResponse> {
    return this.http.post<CustomResultResponse>(
      `${this.baseUrl}/register`,
      payload
    );
  }

  updateStatusRole(
    payload: UpdateUserRequest
  ): Observable<CustomResultResponse> {
    return this.http.put<CustomResultResponse>(
      `${this.baseUrl}/update-status-role`,
      payload
    );
  }

  reset(payload: ResetUserRequest): Observable<CustomResultResponse> {
    return this.http.put<CustomResultResponse>(
      `${this.baseUrl}/reset-password`,
      payload
    );
  }
}
