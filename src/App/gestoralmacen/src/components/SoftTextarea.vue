<template>
  <div class="form-group">
    <label :for="id">
      <slot />
    </label>
    <textarea
      :id="id"
      :value="value"
      class="form-control"
      :rows="rows"
      :placeholder="placeholder"
      @input="$emit('input', $event.target.value)"
    ></textarea>
  </div>
</template>

<script>
export default {
  name: "SoftTextarea",
  props: {
    id: {
      type: String,
      required: true,
    },
    placeholder: {
      type: String,
      default: "Your text here...",
    },
    value: {
      type: String,
      default: "",
    },
    rows: {
      type: String,
      default: "5",
    },
  },
};
</script>
