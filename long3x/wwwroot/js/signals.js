var connection = new signalR.HubConnectionBuilder().withUrl("/UpdateSignals").build();
connection.on("UpdateSignals", UpdateSignals);
connection.on("UpdatePrices", UpdatePrices);
connection.start();

function UpdatePrices(data) {
    $(".price-row").each(function() {
        var coinName = $(this).attr("name");
        $(this).text(data[coinName]);
    });
    console.log(object.values(data));
}

function UpdateSignals(data) {
    $(".data-row").remove();
    var json = jQuery.parseJSON(data);
    console.log(json);
    json.forEach(function (row) {
        var rowHtml = '<tr class="data-row">';
        rowHtml += GetCell(row["ChannelId"]);
        rowHtml += GetCell(row["Coin1"]);
        rowHtml += GetCell(row["Coin2"]);
        rowHtml += GetCell(row["Leverage"]);
        rowHtml += GetCell(row["LongShort"]);
        rowHtml += GetCell(row["EntryZoneMin"]);
        rowHtml += GetCell(row["EntryZoneMax"]);
        rowHtml += GetCell(row["Targets"]);
        rowHtml += GetCell(row["StopLoss"]);
        rowHtml += GetCell(row["TradingType"]);
        rowHtml += GetCell(row["AdditionalInfo"]);
        rowHtml += GetCell(row["Ts"]);
        rowHtml += '<td class="hidden">' + row["FullCoinDescription"] + '</td>';
        rowHtml += "</tr>";
        $("#table").append(rowHtml);
    });
}

function GetCell(value) {
    return "<td>" + value + "</td>";
}