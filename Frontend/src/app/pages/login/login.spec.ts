import { describe, it, expect, beforeEach, vi } from 'vitest';
import { Login } from './login';
import { AuthService } from '../../services/auth.service';
import { Router } from '@angular/router';
import { of, throwError } from 'rxjs';
import { FormsModule } from '@angular/forms';
import { TestBed } from '@angular/core/testing';
import { provideRouter } from '@angular/router';

describe('Login Component', () => {
  let component: Login;
  let authServiceSpy: { login: ReturnType<typeof vi.fn> };
  let router: Router;

  beforeEach(async () => {
    authServiceSpy = { login: vi.fn() };

    await TestBed.configureTestingModule({
      imports: [Login, FormsModule],
      providers: [
        provideRouter([]),
        { provide: AuthService, useValue: authServiceSpy }
      ]
    }).compileComponents();

    const fixture = TestBed.createComponent(Login);
    component = fixture.componentInstance;
    router = TestBed.inject(Router);
    vi.spyOn(router, 'navigate').mockResolvedValue(true);
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
    expect(router.navigate).toHaveBeenCalledWith(['/rent']);
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