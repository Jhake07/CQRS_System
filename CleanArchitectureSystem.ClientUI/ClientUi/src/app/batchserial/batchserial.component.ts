import { Component, OnInit, inject } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environment/environment.dev';
import { BatchSerial } from '../_models/batchSerial';
import { CustomResultResponse } from '../_models/customResultResponse';
import { ToastmessageService } from '../_services/toastmessage.service';
import { ConfirmService } from '../_services/confirm.service';
import { of, timer } from 'rxjs';
import { switchMap, delay, tap } from 'rxjs/operators';

@Component({
  selector: 'app-batchserial',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './batchserial.component.html',
  styleUrl: './batchserial.component.css',
})
export class BatchserialComponent implements OnInit {
  private http = inject(HttpClient);
  private fb = inject(FormBuilder);
  private toast = inject(ToastmessageService);
  private confirmService = inject(ConfirmService);

  batchSerialForm!: FormGroup;
  batchSerialList: BatchSerial[] = [];
  baseUrl = environment.apiUrl;
  isSaving = false;

  ngOnInit(): void {
    this.initializeForm();
    this.loadBatchSerials();
  }

  private initializeForm(): void {
    this.batchSerialForm = this.fb.group({
      contractNo: ['', Validators.required],
      customer: [''],
      address: [''],
      docNo: [''],
      batchQty: [0, Validators.required],
      orderQty: [0],
      deliverQty: [0],
      status: [''],
      serialPrefix: ['', Validators.required],
      startSNo: ['', Validators.required],
      endSNo: ['', Validators.required],
      item_modelCode: ['', Validators.required],
    });
  }

  openConfirmation(): void {
    if (this.batchSerialForm.invalid) {
      this.toast.warning(
        'Please double-check the form before submitting.',
        'Validation Warning'
      );
      this.batchSerialForm.markAllAsTouched();
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
            this.batchSerialForm.disable(); //Lock the form while saving
          }
        }),
        switchMap((confirmed) => (confirmed ? timer(3000) : of(null)))
      )
      .subscribe((tick) => {
        if (tick !== null) {
          this.onSubmit();
        } else {
          this.isSaving = false;
          this.batchSerialForm.enable(); // Re-enable if canceled
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

    this.http
      .post<CustomResultResponse>(
        `${this.baseUrl}batchserial`,
        this.batchSerialForm.value
      )
      .subscribe({
        next: (response) => {
          if (response.isSuccess) {
            this.toast.success(response.message, 'Save Successful');
            this.batchSerialList.push(this.batchSerialForm.value);
            this.batchSerialForm.reset();
          } else {
            this.toast.error(response.message, 'Submission Error');
            this.toast.showValidationWarnings(response.validationErrors);
          }
        },
        error: (err) => {
          this.batchSerialForm.enable();
          const res = err?.error as CustomResultResponse;
          if (res?.message) {
            this.toast.showDetailedError(res);
          } else {
            this.toast.error('Unexpected error format.', 'Error');
          }
        },
        complete: () => {
          this.isSaving = false;
          this.loadBatchSerials();
          this.batchSerialForm.enable();
        },
      });
  }

  private loadBatchSerials(): void {
    this.http.get<BatchSerial[]>(`${this.baseUrl}batchserial`).subscribe({
      next: (data) => (this.batchSerialList = data),
      error: (error) => {
        const msg =
          error.status === 404
            ? 'Batch serials not found. Please check the API.'
            : 'An unexpected error occurred.';
        this.toast.error(msg, 'Load Error');
        console.error('Error fetching batch serials:', error);
      },
    });
  }
}
