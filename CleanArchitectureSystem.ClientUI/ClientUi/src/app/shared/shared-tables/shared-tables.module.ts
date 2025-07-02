import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NoResultsComponent } from '../no-results/no-results.component';
import { PaginatorComponent } from '../paginator/paginator.component';
import { BatchTableComponent } from '../../batchserial/batch-table/batch-table.component';

@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    BatchTableComponent,
    PaginatorComponent,
    NoResultsComponent,
  ],
  exports: [PaginatorComponent, NoResultsComponent, BatchTableComponent],
})
export class SharedTablesModule {}
