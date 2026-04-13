import { Injectable } from '@angular/core';
import { TransactionApi } from '../apis/transactionApi';
import { TransactionResponseDto } from '../models/transaction.model';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class TransactionService {
  constructor(private transactionApi: TransactionApi) {}

  getMyTransactions(customerId: number): Observable<TransactionResponseDto[]> {
    return this.transactionApi.getByCustomer(customerId);
  }

  getTransactionById(id: number): Observable<TransactionResponseDto> {
    return this.transactionApi.getById(id);
  }
}