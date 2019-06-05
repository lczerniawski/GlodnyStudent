import React, { Component } from 'react'

export default class KRScheck extends Component {
    render() {
        return (
            <div>
                 <label>Numer KRS <input type="number" name="KRS" onChange={(e)=>this.props.handleInputChange(e)}/></label> {/* Mozna dac zeby updateowal tylko jak sie KRS zgadza */}
                 <button>Sprawdź</button>
            </div>
        )
    }
}
