import { HttpClient } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { environment } from '../../environment/environment.dev';
import { map } from 'rxjs';
import { AuthResponse } from '../_models/shared/authResponse';
import { ViewUserRequest } from '../_models/appuser/viewUserRequest';

@Injectable({
  providedIn: 'root',
})
export class AccountService {
  private http = inject(HttpClient);
  baseUrl = environment.apiUrl;
  currentUser = signal<ViewUserRequest | null>(null);
  user = this.currentUser();

  setCurrentUser(user: ViewUserRequest) {
    localStorage.setItem('user', JSON.stringify(user));
    this.currentUser.set(user);
  }

  login(model: any) {
    return this.http
      .post<ViewUserRequest>(this.baseUrl + 'userauth/login', model)
      .pipe(
        map((user) => {
          if (user.token == null || user.token == '') {
            alert('Wrong creds');
            this.currentUser.set(null);
          } else {
            this.setCurrentUser(user);
          }
        })
      );

    // return this.http
    //   .post<{ isSuccess: boolean; user?: User; message: string }>(
    //     this.baseUrl + 'userauth/login',
    //     model
    //   )
    //   .pipe(
    //     map((response) => {
    //       if (response.isSuccess && response.user) {
    //         this.setCurrentUser(response.user); // Set current user
    //         alert(response.user);
    //         return response;
    //       } else {
    //         throw new Error(response.message);
    //       }
    //     })
    //   );
  }

  login2(model: any) {
    return this.http
      .post<AuthResponse>(this.baseUrl + 'userauth/login', model)
      .pipe(
        map((response) => {
          if (response && response.isSuccess) {
            const user: ViewUserRequest = response.userDetails; // Extract user details
            this.setCurrentUser(user);
            console.log('Login successful:', user);
          } else {
            console.error('Login failed:', response.message);
          }
        })
      );
  }

  logout() {
    localStorage.removeItem('user');
    this.currentUser.set(null);
  }
}
