import { HttpClient } from '@angular/common/http';
import { Component, inject, OnInit, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { AccountService } from './_services/account.service';
import { NavComponent } from './nav/nav.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, NavComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
})
export class AppComponent implements OnInit {
  title = 'ClientUi';
  private http = inject(HttpClient);
  accountService = inject(AccountService);
  batchSerial = signal<any>(null);
  isUserLogged = signal<boolean>(false); // Use signals for reactive state management

  ngOnInit(): void {
    console.log('app component init');
  }

  getBatchSerial() {
    this.http.get('https://localhost:7290/api/batchserial').subscribe({
      next: (response) => this.batchSerial.set(response),
      error: (error) => console.log(error),
      complete: () => console.log('Request Completed'),
    });

    // Persist login state using the stored user object
    const storedUser = localStorage.getItem('user');
    if (storedUser) {
      this.accountService.setCurrentUser(JSON.parse(storedUser));
      this.isUserLogged.set(true);
    } else {
      this.isUserLogged.set(false);
    }
  }
}
