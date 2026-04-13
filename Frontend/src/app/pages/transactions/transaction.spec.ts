import { describe, it, expect, beforeEach, vi } from 'vitest';
import { Transactions } from './transactions';
import { TransactionService } from '../../services/transaction.service';
import { TestBed } from '@angular/core/testing';
import { of, throwError } from 'rxjs';
import { RouterTestingModule } from '@angular/router/testing';
import { Router } from '@angular/router';

describe('Transactions Component', () => {
  let component: Transactions;
  let fixture: any;
  let serviceSpy: { getMyTransactions: ReturnType<typeof vi.fn> };
  let router: Router;

  beforeEach(async () => {
    serviceSpy = { getMyTransactions: vi.fn() };

    await TestBed.configureTestingModule({
      imports: [Transactions, RouterTestingModule.withRoutes([])],
      providers: [
        { provide: TransactionService, useValue: serviceSpy }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(Transactions);
    component = fixture.componentInstance;
    router = TestBed.inject(Router);
  });

  it('should redirect if no user logged in', async () => {
    localStorage.removeItem('currentUserId');
    const navigateSpy = vi.spyOn(router, 'navigate');

    component.ngOnInit();
    fixture.detectChanges();

    expect(navigateSpy).toHaveBeenCalledWith(['/login']);
    expect(component.loading).toBe(false);
  });

  it('should load transactions successfully', async () => {
    localStorage.setItem('currentUserId', '1');

    const mockData = [
      {
        id: 1,
        customerId: 1,
        customerEmail: 'test@test.com',
        motorId: 1,
        motorBrand: 'Honda',
        daysRented: 2,
        totalAmount: 150,
        dateRented: new Date().toISOString()
      }
    ];
    serviceSpy.getMyTransactions.mockReturnValue(of(mockData));

    component.ngOnInit();
    fixture.detectChanges();

    expect(component.transactions.length).toBe(1);
    expect(component.error).toBe('');
  });

  it('should set error if loading fails', async () => {
    localStorage.setItem('currentUserId', '1');

    serviceSpy.getMyTransactions.mockReturnValue(
      throwError(() => ({ error: { message: 'Failed' } }))
    );

    component.ngOnInit();
    fixture.detectChanges();

    expect(component.error).toBe('Failed');
    expect(component.transactions.length).toBe(0);
  });
});