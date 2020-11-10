<template>
  <solar-modal>
    <template v-slot:header>
      Receive Shipment
    </template>

    <template v-slot:body>
      <label for="product">Product received:</label>
      <select v-model="selectedProduct" class="shipmentItems" id="product">
        <option disabled value="">Please select one</option>
        <option v-for="item in inventory" :key="item.product.id" value="item">
          {{ item.product.name }}
        </option>
      </select>
      <label for="qtyReceived" class="label-body">Quantity Received:</label>
      <input type="number" id="qtyReceived" v-model="qtyReceived" />
    </template>

    <template v-slot:footer>
      <solar-button
        type="button"
        @click.native="save"
        aria-label="Save New Shipment"
      >
        Save Received Shipment
      </solar-button>
      <solar-button
        type="button"
        @click.native="close"
        aria-label="Close Modal"
      >
        Close
      </solar-button>
    </template>
  </solar-modal>
</template>

<script lang="ts">
import { Component, Prop, Vue } from 'vue-property-decorator'
import SolarButton from '@/components/SolarButton.vue'
import SolarModal from '@/components/modals/SolarModal.vue'
import { IProduct, IProductInventory } from '@/interfaces/Product'
import { IShipment } from '@/interfaces/Shipment'
@Component({
  name: 'ShipmentModal',
  components: { SolarButton, SolarModal },
})
export default class ShipmentModal extends Vue {
  @Prop({ required: false, type: Array as () => IProductInventory[] })
  inventory?: IProductInventory[]

  selectedProduct: IProduct = {
    id: 0,
    createdOn: new Date(),
    updatedOn: new Date(),
    name: 'Coffee',
    description: '',
    price: 0,
    isTaxable: false,
    isArchived: true,
  }

  qtyReceived: number = 0

  close() {
    this.$emit('close')
  }

  save() {
    let shipment: IShipment = {
      productId: this.selectedProduct.id,
      adjustment: this.qtyReceived,
    }
    this.$emit('save:shipment', shipment)
  }
}
</script>

<style lang="scss" scoped>
.label-body {
  margin-top: 10px;
}

input {
  width: 98%;
}
</style>
