<template>
  <div class="invoice">
    <h1 class="title">Create Invoice</h1>
    <hr />

    <div class="invoice-step-actions">
      <solar-button @button:click="prev" :disabled="!canGoPrev"
        >Previous</solar-button
      >
      <solar-button @button:click="next" :disabled="!canGoNext"
        >Next</solar-button
      >
      <solar-button @button:click="startOver">Start Over</solar-button>
    </div>
    <hr />

    <div class="invoice-step" v-if="invoiceStep === 1">
      <h2>Step 1: Select Customer</h2>
      <div v-if="customers.length" class="invoice-step-detail">
        <label for="customer">Customer:</label>
        <select
          v-model="selectedCustomerId"
          id="customer"
          class="invoice-customers"
        >
          <option disabled value="">Please select a Customer</option>
          <option v-for="c in customers" :key="c.id" :value="c.id">{{
            c.firstName + ' ' + c.lastName
          }}</option>
        </select>
      </div>
    </div>

    <div class="invoice-step" v-if="invoiceStep === 2">
      <h2>Step 2: Create Order</h2>
      <div v-if="inventory.length" class="invoice-step-detail">
        <label for="product">Product:</label>
        <select v-model="newItem.product" id="product">
          <option disabled value="">Please select a Product</option>
          <option
            v-for="i in inventory"
            :key="i.product.id"
            :value="i.product"
            >{{ i.product.name }}</option
          >
        </select>
        <label for="quantity">Quantity:</label>
        <input type="number" min="0" id="quantity" v-model="newItem.quantity" />
      </div>
      <hr />
      <div class="invoice-step-actions">
        <solar-button
          :disabled="!newItem.product || !newItem.quantity"
          @button:click="addLineItem"
          >Add Line Item</solar-button
        >
        <solar-button
          :disabled="!lineItems.length"
          @button:click="finalizeOrder"
          >Finalize Order</solar-button
        >
      </div>

      <div class="invoice-order-list" v-if="lineItems.length">
        <div class="runningTotal">
          <h3>Running Total:</h3>
          {{ runningTotal | price }}
        </div>
        <hr />
        <table class="table">
          <thead>
            <tr>
              <th>Product</th>
              <th>Description</th>
              <th>Qty.</th>
              <th>Price</th>
              <th>Subtotal</th>
            </tr>
            <tr
              v-for="lineItem in lineItems"
              :key="`index_${lineItem.product.id}`"
            >
              <td>{{ lineItem.product.name }}</td>
              <td>{{ lineItem.product.description }}</td>
              <td>{{ lineItem.quantity }}</td>
              <td>{{ lineItem.product.price }}</td>
              <td>
                {{ (lineItem.product.price * lineItem.quantity) | price }}
              </td>
            </tr>
          </thead>
        </table>
      </div>
    </div>

    <div class="invoice-step" v-if="invoiceStep === 3"></div>
  </div>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator'
import { IInvoice, ILineItems } from '@/interfaces/Invoice'
import { ICustomer } from '@/interfaces/Customer'
import { IProductInventory } from '@/interfaces/Product'
import { CustomerService } from '@/services/customer-service'
import { InventoryService } from '@/services/inventory-service'
import { InvoiceService } from '@/services/invoice-service'
import SolarButton from '@/components/SolarButton.vue'

const customerService = new CustomerService()
const inventoryService = new InventoryService()
const invoiceService = new InvoiceService()

@Component({
  name: 'CreateInvoice',
  components: { SolarButton },
})
export default class CreateInvoice extends Vue {
  invoiceStep: number = 1
  invoice: IInvoice = {
    customerId: 0,
    createdOn: new Date(),
    updatedOn: new Date(),
    lineItems: [],
  }
  customers: ICustomer[] = []
  selectedCustomerId: number = 0
  inventory: IProductInventory[] = []
  lineItems: ILineItems[] = []
  newItem: ILineItems = { product: undefined, quantity: 0 }

  get runningTotal() {
    return this.lineItems.reduce((a, b) => {
      return b.product !== undefined
        ? a + b['product']['price'] * b['quantity']
        : 0
    }, 0)
  }

  get canGoPrev() {
    return this.invoiceStep !== 1
  }

  get canGoNext() {
    if (this.invoiceStep === 1) {
      return this.selectedCustomerId !== 0
    }
    if (this.invoiceStep === 2) {
      return this.lineItems.length
    }
    if (this.invoiceStep === 3) {
      return false
    }

    return false
  }

  prev(): void {
    if (this.invoiceStep === 1) {
      return
    }
    this.invoiceStep -= 1
  }
  next(): void {
    if (this.invoiceStep === 3) {
      return
    }
    this.invoiceStep += 1
  }
  addLineItem() {
    let newItem: ILineItems = {
      product: this.newItem.product,
      quantity: Number(this.newItem.quantity.toString()),
    }
    let existingItems = this.lineItems.map((item) => item.product?.id)
    if (existingItems.includes(newItem.product?.id)) {
      let lineItem = this.lineItems.find(
        (item) => item.product?.id === newItem.product?.id
      )
      let currentQuantity = Number(lineItem?.quantity)
      let updatedQuantity = (currentQuantity += newItem.quantity)
      if (lineItem != undefined) {
        lineItem.quantity = updatedQuantity
      }
    } else {
      this.lineItems.push(this.newItem)
    }

    this.newItem = { product: undefined, quantity: 0 }
  }
  finalizeOrder() {
    this.invoiceStep = 3
  }
  startOver() {
    this.invoice = {
      customerId: 0,
      lineItems: [],
      createdOn: new Date(),
      updatedOn: new Date(),
    }
    this.lineItems = []
    this.invoiceStep = 1
  }

  async initialize(): Promise<void> {
    this.customers = await customerService.getCustomers()
    this.inventory = await inventoryService.getInventory()
  }

  async created() {
    await this.initialize()
  }
}
</script>

<style lang="scss" scoped>
.invoice-step-detail {
  margin: 1rem 0;
}
.invoice-step-actions {
  display: flex;
}
</style>
