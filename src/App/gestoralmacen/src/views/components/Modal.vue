<template>
  <Transition name="modal">
    <div v-if="show" class="modal-mask">
      <div class="modal-container">
        <div class="modal-header">
          <slot name="header">default header</slot>
          <button
              class="btn btn-danger"
              @click="emitSelectedItems"
            >X</button>
        </div>

        <div class="modal-body">
          <div :style="gridStyle">
            <div 
              v-for="(item, index) in items" 
              :key="index" 
              class="grid-item"
              :class="{ 'selected': selectedItems.includes(item) }"
              @click="selectItem(item)"
            >
              {{ item }}
            </div>
          </div>
        </div>
      </div>
    </div>
  </Transition>
</template>

<script>
export default {
  props: ['show', 'rows', 'columns'],
  data() {
    return {
      selectedItems: []
    }
  },
  computed: {
    items() {
      return Array.from({length: this.rows * this.columns}, (_, i) => i + 1)
    },
    gridStyle() {
      return {
        display: 'grid',
        gridTemplateColumns: `repeat(${this.columns}, 1fr)`,
        gridGap: '10px'
      }
    }
  },
  methods: {
    selectItem(item) {
      const index = this.selectedItems.indexOf(item);
      if (index >= 0) {
        this.selectedItems.splice(index, 1);
      } else {
        this.selectedItems.push(item);
      }
    },
    emitSelectedItems() {
      this.$emit('close');
      this.$emit('selected-items', this.selectedItems);
    }
  }
}
</script>

<style>
.grid-item {
  border: 2px solid #82d616; /* Borde de las casillas */
  padding: 10px; /* Espacio interior de las casillas */
  border-radius: 10px;
}

.grid-item.selected {
  background-color: #82d616;
}


.modal-mask {
    position: fixed;
    z-index: 9998;
    top: 0;
    left: 0;
    width: 100%;
    height: 100%;
    background-color: rgba(0, 0, 0, 0.5);
    display: flex;
    transition: opacity 0.3s ease;
  }
  
  .modal-container {
    width: 70%;
    margin: auto;
    padding: 20px 30px;
    background-color: #fff;
    border-radius: 2px;
    box-shadow: 0 2px 8px rgba(0, 0, 0, 0.33);
    transition: all 0.3s ease;
  }
  
  .modal-header h3 {
    margin-top: 0;
    color: #42b983;
  }
  
  .modal-body {
    margin: 20px 0;
  }
  
  .modal-default-button {
    float: right;
  }
  
  /*
   * The following styles are auto-applied to elements with
   * transition="modal" when their visibility is toggled
   * by Vue.js.
   *
   * You can easily play with the modal transition by editing
   * these styles.
   */
  
  .modal-enter-from {
    opacity: 0;
  }
  
  .modal-leave-to {
    opacity: 0;
  }
  
  .modal-enter-from .modal-container,
  .modal-leave-to .modal-container {
    -webkit-transform: scale(1.1);
    transform: scale(1.1);
  }
</style>
