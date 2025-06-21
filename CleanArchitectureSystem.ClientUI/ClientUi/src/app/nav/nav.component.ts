import { HttpClient } from '@angular/common/http';
import { Component, inject } from '@angular/core';
import { AccountService } from '../_services/account.service';
import { FormsModule } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';

@Component({
  selector: 'app-nav',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './nav.component.html',
  styleUrl: './nav.component.css',
})
export class NavComponent {
  accountService = inject(AccountService);
  private router = inject(Router);
  private http = inject(HttpClient);
  private toastr = inject(ToastrService);
  model: any = {};

  accountlogin() {
    console.log('login click');
    this.accountService.login(this.model).subscribe({
      next: () => {
        this.router.navigateByUrl('/home');
      },
      error: (error) => this.toastr.error(error.error),
    });
  }

  registration() {
    console.log('registration click');
  }
}
