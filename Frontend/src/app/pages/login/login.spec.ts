import { ComponentFixture, TestBed } from '@angular/core/testing';
import { Login } from './login';
import { AuthService } from '../../services/auth.service';
import { Router } from '@angular/router';
import { of, throwError } from 'rxjs';
import { FormsModule } from '@angular/forms';
import { provideRouter } from '@angular/router';

describe('Login Component', () => {
  let component: Login;
  let fixture: ComponentFixture<Login>;
  let authServiceSpy: jasmine.SpyObj<AuthService>;
  let routerSpy: jasmine.SpyObj<Router>;

  beforeEach(async () => {
    authServiceSpy = jasmine.createSpyObj('AuthService', ['login']);
    routerSpy = jasmine.createSpyObj('Router', ['navigate']);

    await TestBed.configureTestingModule({
      imports: [Login, FormsModule],
      providers: [
        provideRouter([]),
        { provide: AuthService, useValue: authServiceSpy },
        { provide: Router, useValue: routerSpy }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(Login);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should show error if form is invalid', () => {
    component.onSubmit(undefined as any);
    expect(component.error).toBe('Please fill out the form correctly.');
  });

  it('should login successfully and navigate', () => {
    const mockUser = { id: 1, email: 'test@test.com' };
    authServiceSpy.login.and.returnValue(of(mockUser));

    component.email = 'test@test.com';
    component.password = '1234';
    component.onSubmit({ valid: true } as any);

    expect(localStorage.getItem('currentUserId')).toBe('1');
    expect(routerSpy.navigate).toHaveBeenCalledWith(['/rent']);
  });

  it('should show error on login failure', () => {
    authServiceSpy.login.and.returnValue(
      throwError(() => ({ error: { message: 'Invalid credentials' } }))
    );

    component.email = 'test@test.com';
    component.password = '1234';
    component.onSubmit({ valid: true } as any);

    expect(component.error).toBe('Invalid credentials');
  });
});