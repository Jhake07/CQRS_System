import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { AccountService } from '../../_services/account.service';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './header.component.html',
  styleUrl: './header.component.css',
})
export class HeaderComponent {
  constructor(private accountService: AccountService, private router: Router) {}

  onLogout(): void {
    if (this.accountService.currentUser()) {
      this.accountService.logout();
      this.router.navigate(['/login']);
    }
  }
}
