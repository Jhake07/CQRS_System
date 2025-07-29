import { Component, computed, inject, OnInit } from '@angular/core';
import { RouterOutlet, Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { AccountService } from './_services/account.service';
import { HeaderComponent } from './layout/header/header.component';
import { FooterComponent } from './layout/footer/footer.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, RouterOutlet, HeaderComponent, FooterComponent],
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})
export class AppComponent implements OnInit {
  private router = inject(Router);
  public accountService = inject(AccountService); // public for template access

  isUserLogged = computed(() => this.accountService.currentUser() !== null);
  showSessionWarning = computed(() => this.accountService.sessionWarning$()); //  watch for modal

  ngOnInit(): void {
    const user = this.accountService.getStoredUser();

    if (user) {
      this.accountService.setCurrentUser(user);
      this.router.navigate(['/batchserial']);
    } else {
      this.router.navigate(['/login']);
    }
  }

  onExtendSession(): void {
    this.accountService.extendSession(); // 🕒 user chooses to stay logged in
  }

  onForceLogout(): void {
    this.accountService.logout();
    this.router.navigate(['/login']);
  }
}
