// import { Injectable } from '@angular/core';
// import { AuthApi } from '../apis/authApi';
// import { LoginRequest, CustomerResponseDto } from '../models/auth.model';
// import { tap } from 'rxjs/operators';
// import { BehaviorSubject, Observable } from 'rxjs';

// @Injectable({ providedIn: 'root' })
// export class AuthService {
//   private currentUserSubject = new BehaviorSubject<CustomerResponseDto | null>(null);
//   currentUser$ = this.currentUserSubject.asObservable();

//   constructor(private authApi: AuthApi) {}

//   login(email: string, password: string) {
//     const request: LoginRequest = { email, password };
//     return this.authApi.login(request).pipe(
//       tap(user => this.currentUserSubject.next(user))
//     );
//   }

//   logout() {
//     this.currentUserSubject.next(null);
//   }

//   get currentUser() {
//     return this.currentUserSubject.value;
//   }
// }

import { Injectable } from '@angular/core';
import { AuthApi } from '../apis/authApi';
import { LoginRequest, SignupRequest, CustomerResponseDto } from '../models/auth.model';
import { tap } from 'rxjs/operators';
import { BehaviorSubject, Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private currentUserSubject = new BehaviorSubject<CustomerResponseDto | null>(null);
  currentUser$ = this.currentUserSubject.asObservable();

  constructor(private authApi: AuthApi) {}

  login(email: string, password: string) {
    const request: LoginRequest = { email, password };
    return this.authApi.login(request).pipe(
      tap(user => this.currentUserSubject.next(user))
    );
  }

  signup(email: string, password: string) {
    const request: SignupRequest = { email, password };
    return this.authApi.signup(request).pipe(
      tap(user => this.currentUserSubject.next(user))
    );
  }

  logout() {
    this.currentUserSubject.next(null);
  }

  get currentUser() {
    return this.currentUserSubject.value;
  }
}