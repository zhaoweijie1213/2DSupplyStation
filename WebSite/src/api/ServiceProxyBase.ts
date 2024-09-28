import type { AxiosInstance, AxiosRequestConfig, AxiosResponse } from "axios";
import axios from "axios";

export class ServiceProxyBase {
  protected instance: AxiosInstance;
  // protected baseUrl: string;

  constructor() {
    // 创建 Axios 实例
    this.instance = axios.create({
      baseURL: import.meta.env.VITE_GLOB_API_URL,
      headers: {
        "Content-Type": "application/json",
        "Cache-Control": "no-cache",
      },
    });
  }

  protected async transformOptions(options: AxiosRequestConfig) {
    return options;
  }

  protected async transformResult(
    _url: string,
    response: AxiosResponse,
    processor: (response: AxiosResponse) => Promise<any>
  ): Promise<any> {
    return processor(response);
  }
}
