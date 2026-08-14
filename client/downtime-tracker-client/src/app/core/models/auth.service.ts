import { Injectable, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { Observable, tap } from 'rxjs';
import { environment } from '../../../environments/environment';
import { LoginRequest, LoginResponse } from '../models/auth.model';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private tokenKey = 'downtime_tracker_token';

  currentUsername = signal<string | null>(null);
  currentRole = signal<string | null>(null);

  constructor(private http: HttpClient, private router: Router) {
    this.restoreSession();
  }

  login(request: LoginRequest): Observable<LoginResponse> {
    return this.http.post<LoginResponse>(`${environment.apiUrl}/auth/login`, request).pipe(
      tap(response => {
        sessionStorage.setItem(this.tokenKey, response.token);
        this.currentUsername.set(response.username);
        this.currentRole.set(response.role);
      })
    );
  }

  logout(): void {
    sessionStorage.removeItem(this.tokenKey);
    this.currentUsername.set(null);
    this.currentRole.set(null);
    this.router.navigate(['/login']);
  }

  getToken(): string | null {
    return sessionStorage.getItem(this.tokenKey);
  }

  isLoggedIn(): boolean {
    return !!this.getToken();
  }

  private restoreSession(): void {
    // On page refresh, we still have the token but lost the username/role signals.
    // For now this just confirms a token exists; decoding it for display name is a nice-to-have later.
  }
}