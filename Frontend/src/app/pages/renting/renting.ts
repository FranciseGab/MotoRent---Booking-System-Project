import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { RentService } from '../../services/rent.service';
import { MotorResponseDto } from '../../models/rent.model';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Navbar } from '../../components/navbar/navbar';

type MotorWithState = MotorResponseDto & { days: number; message?: string };

@Component({
  selector: 'app-rent',
  standalone: true,
  imports: [CommonModule, FormsModule,Navbar],
  templateUrl: './renting.html',
  styleUrls: ['./renting.css']
})
export class Rent implements OnInit {
  motors: MotorWithState[] = [];
  loading = false;
  error = '';

  constructor(
    private rentService: RentService,
    private cdr: ChangeDetectorRef // ← add this
  ) {}

  ngOnInit() {
    this.loadMotors();
  }

  loadMotors() {
    this.loading = true;
    this.error = '';
    this.motors = [];

    this.rentService.getAvailableMotors().subscribe({
      next: (motors) => {
        this.motors = motors.map(m => ({ ...m, days: 1, message: '' }));
        this.loading = false;
        this.cdr.detectChanges(); // ← force UI update after navigation
      },
      error: (err) => {
        console.error('Error loading motors:', err);
        this.error = err.error?.message || 'Failed to load motors.';
        this.loading = false;
        this.cdr.detectChanges(); // ← force UI update on error too
      },
      complete: () => {
        this.loading = false;
        this.cdr.detectChanges(); // ← safety net
      }
    });
  }

  rentMotor(motor: MotorWithState) {
    const customerId = Number(localStorage.getItem('currentUserId'));
    if (!customerId) {
      motor.message = 'Please login first';
      return;
    }

    this.rentService.rentMotor({ customerId, motorId: motor.id, days: motor.days })
      .subscribe({
        next: (tx) => {
          motor.message = `Rented! Total: $${tx.totalAmount}`;
          motor.isAvailable = false;
          this.cdr.detectChanges(); // ← update card state immediately
        },
        error: (err) => {
          motor.message = err.error?.message || 'Rent failed';
          console.error(err);
          this.cdr.detectChanges();
        }
      });
  }

  canRent(motor: MotorWithState) {
    return motor.isAvailable && motor.days > 0;
  }
}