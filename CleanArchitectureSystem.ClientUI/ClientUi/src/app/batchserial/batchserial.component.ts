import { Component, OnInit, inject } from '@angular/core';
import { FormGroup } from '@angular/forms';

import { of, timer } from 'rxjs';
import { switchMap, tap, finalize } from 'rxjs/operators';

import { BatchSerialFormFactory } from '../_formfactories/batch-serial-form.service';
import { BatchSerialService } from '../_services/batch-serial.service';
import { ConfirmService } from '../_services/confirm.service';
import { ToastmessageService } from '../_services/toastmessage.service';
import { FormAccessService } from '../_services/form-access.service';
import { FormUtilsService } from '../_services/form-utils.service';
import { PaginatorService } from '../_services/paginator.service';

import { BatchSerial } from '../_models/batchSerial';
import { CustomResultResponse } from '../_models/customResultResponse';
import { FormMode } from '../_enums/form-mode.enum';
import { Status } from '../_enums/status.enum';

import { SharedFormsModule } from '../shared/shared-forms/shared-forms.module';
import { SharedTablesModule } from '../shared/shared-tables/shared-tables.module';

@Component({
  selector: 'app-batchserial',
  standalone: true,
  imports: [SharedFormsModule, SharedTablesModule],
  templateUrl: './batchserial.component.html',
  styleUrl: './batchserial.component.css',
})
export class BatchserialComponent implements OnInit {
  private batchService = inject(BatchSerialService);
  private toast = inject(ToastmessageService);
  private confirmService = inject(ConfirmService);
  private formFactory = inject(BatchSerialFormFactory);
  private formAccess = inject(FormAccessService);
  private formUtils = inject(FormUtilsService);
  private paginator = inject(PaginatorService);

  StatusType = Status;
  FormMode = FormMode;
  statusOptions: string[] = Object.values(Status);

  batchSerialForm!: FormGroup;
  batchSerialList: BatchSerial[] = [];
  selectedBatchSerial: BatchSerial | null = null;

  formMode: FormMode = FormMode.None;
  isSaving = false;
  showValidationAlert = false;

  pageSize = 5;
  currentPage = 1;

  sortColumn: string = '';
  sortDirection: '' | 'asc' | 'desc' = '';

  readonly editableFields: string[] = [
    'contractNo',
    'customer',
    'address',
    'docNo',
    'item_ModelCode',
  ];

  ngOnInit(): void {
    this.formMode = FormMode.New;
    this.initializeForm();
    this.loadBatchSerials();
  }

  private initializeForm(): void {
    this.batchSerialForm = this.formFactory.create(this.formMode);
    this.applyFieldAccess();
  }

  private loadBatchSerials(): void {
    this.batchService.getAll().subscribe({
      next: (data) => {
        this.batchSerialList = data;
      },
      error: (error) => {
        const msg =
          error.status === 404
            ? 'Batch serials not found. Please check the API.'
            : 'An unexpected error occurred.';
        this.toast.error(msg, 'Load Error');
      },
    });
  }

  openConfirmation(): void {
    this.showValidationAlert = false;

    if (this.batchSerialForm.invalid) {
      this.formUtils.markAllTouched(this.batchSerialForm);
      this.showValidationAlert = true;
      return;
    }

    this.confirmService
      .confirm(
        'Confirm Save',
        'Are you sure you want to save this batch serial entry?',
        'Yes, Save',
        'Cancel'
      )
      .pipe(
        tap((confirmed) => {
          if (confirmed) {
            this.isSaving = true;
            this.batchSerialForm.disable();
          }
        }),
        switchMap((confirmed) => (confirmed ? timer(2000) : of(null)))
      )
      .subscribe((tick) => {
        if (tick !== null) {
          this.onSubmit();
        } else {
          this.isSaving = false;
        }
      });
  }

  onSubmit(): void {
    if (this.batchSerialForm.invalid) {
      this.toast.warning(
        'Please double-check the form before submitting.',
        'Validation Warning'
      );
      return;
    }

    this.isSaving = true;
    this.batchSerialForm.disable();

    const payload = this.batchSerialForm.getRawValue();
    const request$ =
      this.formMode === FormMode.Edit && this.selectedBatchSerial
        ? this.batchService.update(this.selectedBatchSerial.id!, payload)
        : this.batchService.save(payload);

    request$.subscribe({
      next: (response) => {
        if (response.isSuccess) {
          this.toast.success(
            response.message,
            this.formMode === FormMode.Edit
              ? 'Update Successful'
              : 'Save Successful'
          );

          if (this.formMode === FormMode.Edit) {
            const index = this.batchSerialList.findIndex(
              (x) => x.contractNo === this.selectedBatchSerial?.contractNo
            );
            if (index !== -1)
              this.batchSerialList[index] = this.batchSerialForm.value;
          } else {
            this.batchSerialList.push(this.batchSerialForm.value);
          }

          this.resetForm();
        } else {
          this.toast.error(response.message, 'Submission Error');
          this.toast.showValidationWarnings(response.validationErrors);
        }
      },
      error: (err) => {
        this.batchSerialForm.enable();
        const res = err?.error as CustomResultResponse;
        res?.message
          ? this.toast.showDetailedError(res)
          : this.toast.error('Unexpected error format.', 'Error');
        this.isSaving = false;
      },
      complete: () => {
        this.isSaving = false;
        this.loadBatchSerials();
        this.batchSerialForm.enable();
      },
    });
  }

  populateFormEdit(batch: BatchSerial): void {
    this.formMode = FormMode.Edit;
    this.batchSerialForm.enable();
    this.applyFieldAccess();
    this.batchSerialForm.patchValue(batch);
    this.selectedBatchSerial = batch;
  }

  populateFormView(batch: BatchSerial): void {
    this.formMode = FormMode.View;
    this.batchSerialForm.disable();
    this.batchSerialForm.patchValue(batch);
    this.selectedBatchSerial = batch;
  }

  resetForm(): void {
    this.formMode = FormMode.New;
    this.selectedBatchSerial = null;
    this.showValidationAlert = false;

    this.formUtils.resetWithDefaults(this.batchSerialForm, {
      id: null,
      orderQty: 0,
      deliverQty: 0,
      status: 'Open',
      batchQty: 0,
    });
  }

  cancelBatchSerial(id: number): void {
    this.confirmService
      .confirm(
        'Confirm Cancellation',
        'Are you sure you want to cancel this batch contract?',
        'Yes',
        'No'
      )
      .pipe(
        switchMap((confirmed) => {
          if (!confirmed) return of(null);
          this.isSaving = true;
          return this.batchService.cancel(id);
        }),
        finalize(() => {
          this.isSaving = false;
        })
      )
      .subscribe({
        next: (response) => {
          if (!response) return;

          if (response.isSuccess) {
            this.toast.success(response.message, 'Cancelled');
            this.resetForm();
          } else {
            this.toast.error(response.message, 'Cancellation Failed');
            this.toast.showValidationWarnings(response.validationErrors);
          }

          this.loadBatchSerials();
        },
        error: (err) => {
          const res = err?.error as CustomResultResponse;
          res?.message
            ? this.toast.showDetailedError(res)
            : this.toast.error('Unexpected error format.', 'Error');
        },
      });
  }

  applyFieldAccess(): void {
    this.formAccess.applyAccess(
      this.batchSerialForm,
      this.formMode,
      this.editableFields
    );
  }

  allowOnlyDigits(event: KeyboardEvent): void {
    const key = event.key;
    if (!/^\d$/.test(key)) {
      event.preventDefault();
    }
  }

  handleSort(event: { column: string; direction: '' | 'asc' | 'desc' }): void {
    this.sortColumn = event.column;
    this.sortDirection = event.direction;
  }

  get paginatedBatchList(): BatchSerial[] {
    let sorted = [...this.batchSerialList];

    if (this.sortColumn && this.sortDirection) {
      sorted.sort((a, b) => {
        const valA = a[this.sortColumn as keyof BatchSerial];
        const valB = b[this.sortColumn as keyof BatchSerial];

        if (valA == null || valB == null) return 0;
        return this.sortDirection === 'asc'
          ? valA > valB
            ? 1
            : -1
          : valA < valB
          ? 1
          : -1;
      });
    }

    return this.paginator.getPaginated(sorted, this.pageSize, this.currentPage);
  }

  get totalPages(): number {
    return this.paginator.getTotalPages(this.batchSerialList, this.pageSize);
  }
}
