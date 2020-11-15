
    $(function() {
        $("#search").autocomplete({
            minLength: 0,
            source: function (request, response) {
                $.ajax({
                    url: 'Product/Search',
                    type: 'POST',
                    dataType: 'JSON',
                    data: { value: $('') },
                    sussess: function (data) {
                        response($.map(data, function (item) {
                            console.log(item);
                        }));
                    },
                });
                },
			focus: function( event, ui ) {
        $("#project").val(ui.item.label);
                return false;
			},
			select: function( event, ui ) {
        $("#project").val(ui.item.label);
    $( "#project-id" ).val( ui.item.value );
				$( "#project-description" ).html( ui.item.desc );
				$( "#project-icon" ).attr( "src", "images/" + ui.item.icon );
				return false;
			}
		})
		.data( "autocomplete" )._renderItem = function( ul, item ) {
			return $( "<li></li>" )
				.data( "item.autocomplete", item )
				.append( "<a>" + item.label + "<br>" + item.desc + "</a>" )
				.appendTo( ul );
		};
	});