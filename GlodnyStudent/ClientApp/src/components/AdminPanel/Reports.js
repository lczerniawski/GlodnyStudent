import React, { Component } from 'react';


export default class Reports extends Component {

    render() {
        const reportsList = this.props.reports?this.props.reports.map(report=><li id={report.id}>
            <p>Informacja o zgłoszeniu: {report.content}</p>
            <p>Link:<a href={`/Restauracja/${report.restaurantId}`} >Przejdz do restauracji</a></p>
            <button value={report.id} onClick={(e)=>this.props.removeReport(e)} >X</button>
            </li>):<li>Brak zgłoszeń</li>;
        return (
            <div>
                <h3>Zgłoszenia</h3>
                <button onClick={this.props.getReports} >Odświerz listę zgłoszeń</button>
                <ul>
                    {reportsList}
                </ul>
            </div>
        )
    }
}
