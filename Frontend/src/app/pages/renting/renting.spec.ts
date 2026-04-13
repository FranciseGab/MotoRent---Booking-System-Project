import { describe, it, expect, beforeEach, vi } from 'vitest';
import { Rent } from './renting';
import { RentService } from '../../services/rent.service';
import { TestBed } from '@angular/core/testing';
import { of, throwError } from 'rxjs';
import { FormsModule } from '@angular/forms';
import { RouterTestingModule } from '@angular/router/testing';

describe('Rent Component', () => {
  let component: Rent;
  let rentServiceSpy: { getAvailableMotors: ReturnType<typeof vi.fn>; rentMotor: ReturnType<typeof vi.fn> };

  beforeEach(() => {
    rentServiceSpy = {
      getAvailableMotors: vi.fn(),
      rentMotor: vi.fn()
    };

    TestBed.configureTestingModule({
      imports: [Rent, FormsModule, RouterTestingModule],
      providers: [{ provide: RentService, useValue: rentServiceSpy }]
    });

    const fixture = TestBed.createComponent(Rent);
    component = fixture.componentInstance;
  });

  it('should load motors on init', () => {
    const mockMotors = [{ id: 1, brand: 'Yamaha', pricePerDay: 50, isAvailable: true }];
    rentServiceSpy.getAvailableMotors.mockReturnValue(of(mockMotors));

    component.ngOnInit();

    expect(component.motors.length).toBe(1);
    expect(component.loading).toBe(false);
  });

  it('should set error if loading motors fails', () => {
    rentServiceSpy.getAvailableMotors.mockReturnValue(
      throwError(() => ({ error: { message: 'Server error' } }))
    );

    component.loadMotors();

    expect(component.error).toBe('Server error');
    expect(component.loading).toBe(false);
  });

  it('canRent should return true only if available and days > 0', () => {
    const motor: any = { isAvailable: true, days: 2 };
    expect(component.canRent(motor)).toBe(true);

    motor.days = 0;
    expect(component.canRent(motor)).toBe(false);
  });

  it('should rent motor successfully', () => {
    localStorage.setItem('currentUserId', '1');
    const motor: any = { id: 1, days: 2, isAvailable: true };

    rentServiceSpy.rentMotor.mockReturnValue(
      of({
        id: 1,
        customerId: 1,
        customerEmail: 'test@test.com',
        motorId: 1,
        motorBrand: 'Yamaha',
        daysRented: 2,
        totalAmount: 100,
        dateRented: new Date().toISOString()
      })
    );

    component.rentMotor(motor);

    expect(motor.isAvailable).toBe(false);
    expect(motor.message).toContain('Rented!');
  });

  it('should show message if not logged in', () => {
    localStorage.removeItem('currentUserId');
    const motor: any = { id: 1, days: 1, isAvailable: true };

    component.rentMotor(motor);

    expect(motor.message).toBe('Please login first');
  });
});