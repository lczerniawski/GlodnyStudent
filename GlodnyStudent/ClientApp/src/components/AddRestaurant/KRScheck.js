import React, { Component } from 'react';
import axios from 'axios';

export default class KRScheck extends Component {
    state = {
        count: null,
        error: null
    }

    onChange = (e) => {
        const number = e.target.value;
        fetch(`https://api-v3.mojepanstwo.pl/dane/krs_podmioty.json?conditions[krs_podmioty.krs]=${number}`)
            .then(res => res.json())
            .then(data => this.setState({
                count: data.Count
            }))
            .catch(err => console.log(err))
    }

    render() {
        return (
            <div>
                 <label>Numer KRS <input type="number" name="KRS" onChange={this.onChange}/></label> {/* Mozna dac zeby updateowal tylko jak sie KRS zgadza */}
                 <button>Sprawdź</button>
                 <p>{this.state.count}</p>
            </div>
        )
    }
}
