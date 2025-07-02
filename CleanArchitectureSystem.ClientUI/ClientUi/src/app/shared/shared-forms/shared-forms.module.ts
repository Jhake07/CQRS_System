import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { FieldAccessDirective } from '../../_directives/field-access.directive';
import { StatusClassPipe } from '../../_pipes/status-class.pipe';
import { PaginatorComponent } from '../paginator/paginator.component';

@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    FieldAccessDirective,
    StatusClassPipe,
    PaginatorComponent,
    ReactiveFormsModule,
  ],
  exports: [
    ReactiveFormsModule,
    FieldAccessDirective,
    StatusClassPipe,
    PaginatorComponent,
  ],
})
export class SharedFormsModule {}
