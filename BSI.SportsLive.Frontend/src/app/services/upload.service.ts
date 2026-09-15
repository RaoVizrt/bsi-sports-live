import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export const MAX_PHOTO_SIZE_BYTES = 500 * 1024; // 500 KB
export const ALLOWED_PHOTO_TYPES = ['image/jpeg', 'image/png'];

@Injectable({
  providedIn: 'root'
})
export class UploadService {
  private apiUrl = 'http://localhost:5258/api/uploads';

  constructor(private http: HttpClient) {}

  uploadPlayerPhoto(file: File): Observable<{ url: string }> {
    const formData = new FormData();
    formData.append('file', file);
    return this.http.post<{ url: string }>(`${this.apiUrl}/player-photo`, formData);
  }

  validateFile(file: File): string | null {
    if (!ALLOWED_PHOTO_TYPES.includes(file.type)) {
      return 'Only JPG and PNG images are allowed.';
    }
    if (file.size > MAX_PHOTO_SIZE_BYTES) {
      return 'File too large. Maximum allowed size is 500 KB.';
    }
    return null;
  }
}