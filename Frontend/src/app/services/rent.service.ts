import { Injectable } from '@angular/core';
import { RentApi } from '../apis/rentApi';
import { MotorResponseDto, RentRequestDto, TransactionResponseDto } from '../models/rent.model';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class RentService {
  constructor(private rentApi: RentApi) {}

  getAvailableMotors(): Observable<MotorResponseDto[]> {
    return this.rentApi.getMotors();
  }

  rentMotor(request: RentRequestDto): Observable<TransactionResponseDto> {
    return this.rentApi.rentMotor(request);
  }
}