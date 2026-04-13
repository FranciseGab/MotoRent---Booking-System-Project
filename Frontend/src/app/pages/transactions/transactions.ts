import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { TransactionService } from '../../services/transaction.service';
import { TransactionResponseDto } from '../../models/transaction.model';
import { CommonModule, DecimalPipe } from '@angular/common';
import { Router } from '@angular/router';
import { Navbar } from '../../components/navbar/navbar';
import { RentApi } from '../../apis/rentApi'; // ✅ use RentApi

@Component({
  selector: 'app-transactions',
  standalone: true,
  imports: [CommonModule, Navbar, DecimalPipe],
  templateUrl: './transactions.html',
  styleUrls: ['./transactions.css']
})
export class Transactions implements OnInit {
  transactions: TransactionResponseDto[] = [];
  loading = false;
  error = '';

  selectedTx: TransactionResponseDto | null = null;
  motorImage: string = '';
  pricePerDay: number = 0;

  constructor(
    private transactionService: TransactionService,
    private router: Router,
    private cdr: ChangeDetectorRef,
    private rentApi: RentApi  // ✅ injected here
  ) {}

  ngOnInit() {
    const customerId = Number(localStorage.getItem('currentUserId'));
    if (!customerId) {
      this.router.navigate(['/login']);
      return;
    }
    this.loadTransactions(customerId);
  }

  loadTransactions(customerId: number) {
    this.loading = true;
    this.error = '';

    this.transactionService.getMyTransactions(customerId).subscribe({
      next: (data) => {
        this.transactions = data;
        this.loading = false;
        this.cdr.detectChanges();
      },
      error: (err) => {
        this.error = err.error?.message || 'Failed to load transactions.';
        this.loading = false;
        this.cdr.detectChanges();
      },
      complete: () => {
        this.loading = false;
        this.cdr.detectChanges();
      }
    });
  }

  openDetail(tx: TransactionResponseDto) {
    this.selectedTx = tx;
    this.motorImage = '';
    this.pricePerDay = 0;

    // ✅ Use rentApi.getMotors() to find image + price for this motor
    this.rentApi.getMotors().subscribe({
      next: (motors) => {
        const motor = motors.find(m => m.id === tx.motorId);
        if (motor) {
          this.motorImage = motor.imageUrl;
          this.pricePerDay = motor.pricePerDay;
        }
        this.cdr.detectChanges();
      }
    });
  }

  closeDetail() {
    this.selectedTx = null;
  }
}