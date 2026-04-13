// import { Injectable } from '@angular/core';
// import { MainApi } from './mainApi';
// import { LoginRequest, CustomerResponseDto } from '../models/auth.model';
// import { Observable } from 'rxjs';

// @Injectable({ providedIn: 'root' })
// export class AuthApi {
//   constructor(private api: MainApi) {}

//   login(request: LoginRequest): Observable<CustomerResponseDto> {
//     return this.api.post<CustomerResponseDto>('customer/login', request);
//   }
// }

import { Injectable } from '@angular/core';
import { MainApi } from './mainApi';
import { LoginRequest, SignupRequest, CustomerResponseDto } from '../models/auth.model';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class AuthApi {
  constructor(private api: MainApi) {}

  login(request: LoginRequest): Observable<CustomerResponseDto> {
    return this.api.post<CustomerResponseDto>('customer/login', request);
  }

  signup(request: SignupRequest): Observable<CustomerResponseDto> {
    return this.api.post<CustomerResponseDto>('customer/signup', request);
  }
}