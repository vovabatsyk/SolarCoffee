<template>
  <div class="customers">
    <h1 class="title">Manage Customers</h1>
    <hr />
    <div class="title-actions">
      <solar-button @button:click="showNewCustomerModal"
        >Add Customer</solar-button
      >
    </div>
    <table class="table">
      <tr>
        <th>Customer</th>
        <th>Address</th>
        <th>State</th>
        <th>Since</th>
        <th>Delete</th>
      </tr>
      <tr v-for="customer in customers" :key="customer.id">
        <td>{{ customer.firstName + ' ' + customer.lastName }}</td>
        <td>
          {{
            customer.primaryAddress.addressLine1 +
              ' ' +
              customer.primaryAddress.addressLine2
          }}
        </td>
        <td>{{ customer.primaryAddress.state }}</td>
        <td>{{ customer.createdOn | humanizeDate }}</td>
        <td>
          <div
            class="lni lni-cross-circle cusromer-delete"
            @click="deleteCustomer(customer.id)"
          ></div>
        </td>
      </tr>
    </table>
    <new-customer-modal
      v-if="isCustomerModalVisible"
      @close="closeModals"
      @save:customer="saveCustomer"
    ></new-customer-modal>
  </div>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator'
import { ICustomer } from '@/interfaces/Customer'
import { CustomerService } from '@/services/customer-service'
import NewCustomerModal from '@/components/modals/NewCustomerModal.vue'
import SolarButton from '@/components/SolarButton.vue'

const customerService = new CustomerService()

@Component({
  name: 'Customers',
  components: { SolarButton, NewCustomerModal },
})
export default class Customers extends Vue {
  customers: ICustomer[] = []
  isCustomerModalVisible: boolean = false

  showNewCustomerModal() {
    this.isCustomerModalVisible = true
  }

  closeModals() {
    this.isCustomerModalVisible = false
  }

  async deleteCustomer(customerId: number) {
    await customerService.deleteCustomer(customerId)
    await this.initialize()
  }

  async saveCustomer(newCustomer: ICustomer) {
    await customerService.addCustomer(newCustomer)
    this.isCustomerModalVisible = false
    await this.initialize()
  }

  async initialize() {
    this.customers = await customerService.getCustomers()
  }

  async created() {
    await this.initialize()
  }
}
</script>

<style lang="scss" scoped>
@import '@/assets/scss/global.scss';

.cusromer-delete {
  cursor: pointer;
  font-weight: bold;
  font-size: 1.2rem;
  color: $solar-red;
}
</style>
