import axios from 'axios'
import { IServiceResponse } from '@/interfaces/ServiceResponse'
import { ICustomer } from '@/interfaces/Customer.d'

export class CustomerService {
  API_URL = process.env.VUE_APP_API_URL

  async getCustomers(): Promise<ICustomer[]> {
    let result: any = await axios.get(`${this.API_URL}/customer/`)
    return result.data
  }

  public async addCustomer(
    newCustomer: ICustomer
  ): Promise<IServiceResponse<ICustomer>> {
    let result: any = await axios.post(`${this.API_URL}/customer/`, newCustomer)
    return result.data
  }

  public async deleteCustomer(customerId: number): Promise<boolean> {
    let result: any = await axios.delete(
      `${this.API_URL}/customer/${customerId}`
    )
    return result.data
  }
}
