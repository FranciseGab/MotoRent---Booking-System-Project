import { describe, it, expect, beforeEach, vi } from 'vitest';
import { Login } from './login';
import { AuthService } from '../../services/auth.service';
import { Router, RouterLink } from '@angular/router';
import { of, throwError } from 'rxjs';
import { FormsModule } from '@angular/forms';
import { TestBed } from '@angular/core/testing';
import { CommonModule } from '@angular/common';

describe('Login Component', () => {
  let component: Login;
  let authServiceSpy: { login: ReturnType<typeof vi.fn> };
  let routerSpy: { navigate: ReturnType<typeof vi.fn> };

  beforeEach(async () => {
    authServiceSpy = { login: vi.fn() };
    routerSpy = { navigate: vi.fn().mockResolvedValue(true) };

    await TestBed.configureTestingModule({
      imports: [Login, FormsModule],
      providers: [
        { provide: AuthService, useValue: authServiceSpy },
        { provide: Router, useValue: routerSpy }
      ]
    })
    .overrideComponent(Login, {
      set: {
        imports: [CommonModule, FormsModule, RouterLink],
        providers: [
          { provide: Router, useValue: routerSpy }
        ]
      }
    })
    .compileComponents();

    const fixture = TestBed.createComponent(Login);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should show error if form is invalid', () => {
    component.onSubmit(undefined as any);
    expect(component.error).toBe('Please fill out the form correctly.');
  });

  it('should login successfully and navigate', () => {
    const mockUser = { id: 1, email: 'test@test.com' };
    authServiceSpy.login.mockReturnValue(of(mockUser));

    component.email = 'test@test.com';
    component.password = '1234';
    component.onSubmit({ valid: true } as any);

    expect(localStorage.getItem('currentUserId')).toBe('1');
    expect(routerSpy.navigate).toHaveBeenCalledWith(['/rent']);
  });

  it('should show error on login failure', () => {
    authServiceSpy.login.mockReturnValue(
      throwError(() => ({ error: { message: 'Invalid credentials' } }))
    );

    component.email = 'test@test.com';
    component.password = '1234';
    component.onSubmit({ valid: true } as any);

    expect(component.error).toBe('Invalid credentials');
  });
});