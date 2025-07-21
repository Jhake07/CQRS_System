import { Component, EventEmitter, Input, Output } from '@angular/core';
import { CommonModule, NgClass } from '@angular/common';
import { StatusClassPipe } from '../../_pipes/status-class.pipe';
import { NoResultsComponent } from '../../shared/no-results/no-results.component';
import { SortableHeaderDirective } from '../../_directives/sortable-header.directive';
import { ViewUserRequest } from '../../_models/appuser/viewUserRequest';
import { UpdateUserRequest } from '../../_models/appuser/updateUserRequest';
import { ResetUserRequest } from '../../_models/appuser/resetUserRequest';

@Component({
  selector: 'app-user-table',
  standalone: true,
  imports: [
    CommonModule,
    NoResultsComponent,
    StatusClassPipe,
    NgClass,
    SortableHeaderDirective,
  ],
  templateUrl: './user-table.component.html',
  styleUrl: './user-table.component.css',
})
export class UserTableComponent {
  @Input() userList: ViewUserRequest[] = [];

  @Output() edit = new EventEmitter<ViewUserRequest>();
  @Output() view = new EventEmitter<ViewUserRequest>();
  @Output() reset = new EventEmitter<ResetUserRequest>();

  @Input() sortColumn: string = '';
  @Input() sortDirection: 'asc' | 'desc' | '' = '';
  @Output() sort = new EventEmitter<{
    column: string;
    direction: '' | 'asc' | 'desc';
  }>();
}
