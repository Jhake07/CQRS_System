import { Component, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule, NgClass } from '@angular/common';
import { BatchSerial } from '../../_models/batchSerial';
import { StatusClassPipe } from '../../_pipes/status-class.pipe';
import { NoResultsComponent } from '../../shared/no-results/no-results.component';
import { SortableHeaderDirective } from '../../_directives/sortable-header.directive';

@Component({
  selector: 'app-batch-table',
  standalone: true,
  imports: [
    CommonModule,
    NoResultsComponent,
    StatusClassPipe,
    NgClass,
    SortableHeaderDirective,
  ],
  templateUrl: './batch-table.component.html',
  styleUrls: ['./batch-table.component.css'],
})
export class BatchTableComponent {
  @Input() batchList: BatchSerial[] = [];

  @Output() edit = new EventEmitter<BatchSerial>();
  @Output() view = new EventEmitter<BatchSerial>();
  @Output() cancel = new EventEmitter<number>();

  @Input() sortColumn: string = '';
  @Input() sortDirection: 'asc' | 'desc' | '' = '';
  @Output() sort = new EventEmitter<{
    column: string;
    direction: '' | 'asc' | 'desc';
  }>();
}
