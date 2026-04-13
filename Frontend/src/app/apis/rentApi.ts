import { Injectable } from '@angular/core';
import { MainApi } from './mainApi';
import { RentRequestDto, TransactionResponseDto, MotorResponseDto } from '../models/rent.model';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class RentApi {
  constructor(private api: MainApi) {}

  getMotors(): Observable<MotorResponseDto[]> {
    return this.api.get<MotorResponseDto[]>('motor');
  }

  rentMotor(request: RentRequestDto): Observable<TransactionResponseDto> {
    return this.api.post<TransactionResponseDto>('motor/rent', request);
  }
}