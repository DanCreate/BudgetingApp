function initializeFormTransactions(urlGetCategories) {

    $("#OperationTypeID").change(async function () {
        const selectedvalue = $(this).val();

        const answer = await fetch(urlGetCategories, {

            method: 'POST',
            body: selectedvalue,
            headers: {
                'Content-Type': 'application/json'
            }

        });

        const json = await answer.json();
        const options = json.map(category => `<option value=${category.value}>${category.text}</option>`);
        $("#CategoryID").html(options);
    })


}