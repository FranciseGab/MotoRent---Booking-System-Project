import { HttpClient, HttpHeaders } from '@angular/common/http';
import { environment } from '../environments/environment';
import { Injectable } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class MainApi {
  constructor(private http: HttpClient) {}

  get<T>(url: string) {
    return this.http.get<T>(`${environment.apiUrl}/${url}`);
  }

  post<T>(url: string, body: any) {
    return this.http.post<T>(`${environment.apiUrl}/${url}`, body);
  }
}