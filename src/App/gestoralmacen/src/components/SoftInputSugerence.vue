<template>
<div class="autocomplete">
    <input 
        v-model="input" 
        @input="update" 
        @keyup.down="down" 
        @keyup.up="up"
        @keyup.enter="hit" 
        @blur="reset" 
        class="input form-control"
        placeholder="Escribe para buscar..."
    />
    
    <div class="suggestions" v-if="open">
        <div 
            v-for="(suggestion, i) in filtered" 
            :key="i"
            @click="select(suggestion)" 
            class="suggestion row" 
            :class="{ active: i === active }" 
        >
            {{ suggestion }}
        </div>
    </div>
</div>
</template>

<script>

export default {
    name: 'soft-input-sugerence',
    props: {
        suggestions: {
            type: Array,
            default: () => {}
        }
    },
    data() {
        return {
            open: false,
            input: "",
            active: 0
        };
    },
    computed: {
        filtered() {
            let results = this.suggestions.filter(suggestion => {
                return suggestion.toLowerCase().indexOf(this.input.toLowerCase()) > -1;
            });

            if (results.length === 0) {
                return ["No se encontraron sugerencias"];
            }

            return results.slice(0, 5);
        }
    },
    methods: {
        update() {
            if (!this.open) {
                this.open = true;
                this.active = 0;
            }
        },
        down() {
            if (this.active < this.filtered.length - 1) {
                this.active++;
            }
        },
        up() {
            if (this.active > 0) {
                this.active--;
            }
        },
        hit() {
            this.select(this.filtered[this.active]);
        },
        select(suggestion) {
            this.input = suggestion;
            this.open = false;
        },
        reset() {
            this.active = 0;
            this.open = false;
        }
    }
};
</script>

<style scoped>
.autocomplete {
    position: relative;
}
.suggestions {
    position: absolute;
    background: white;
    width: 100%;
    z-index: 1;
}
.suggestion {
    padding: 10px;
    cursor: pointer;
}
.suggestion.active {
    background: #f5f5f5;
}
</style>
