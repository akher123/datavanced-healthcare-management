
export interface PaginationRequest {
  keyword?: string;
  sortBy?: string;
  descending: boolean;
  pageIndex: number;
  pageSize: number;
}

export interface ApiResponse<T> {
  result: T;
  message: string;
  isError: boolean;
  statusCode: number;
  errorMessage: string;
}

export interface PagedResult<T> {
  data: T[];
  totalCount: number;
}
