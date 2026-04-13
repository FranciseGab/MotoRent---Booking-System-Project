import { Injectable } from '@angular/core';
import { MainApi } from './mainApi';
import { TransactionResponseDto } from '../models/transaction.model';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class TransactionApi {
  constructor(private api: MainApi) {}

  getByCustomer(customerId: number): Observable<TransactionResponseDto[]> {
    return this.api.get<TransactionResponseDto[]>(`transaction/customer/${customerId}`);
  }

  getById(id: number): Observable<TransactionResponseDto> {
    return this.api.get<TransactionResponseDto>(`transaction/${id}`);
  }
}