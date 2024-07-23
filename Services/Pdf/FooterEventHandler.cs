using System;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Kernel.Events;
using iText.Kernel.Geom;
using iText.Kernel.Font;
using iText.IO.Font.Constants;

public class FooterEventHandler : IEventHandler
{
	private readonly Document _document;
	private readonly PdfFont _font;
	private readonly string _footer;

	public FooterEventHandler(Document document, PdfFont font, string footer)
	{
		_document = document;
		_font = font;
		_footer = footer;
	}

	public void HandleEvent(Event @event)
	{
		PdfDocumentEvent docEvent = (PdfDocumentEvent)@event;
		PdfDocument pdfDoc = docEvent.GetDocument();
		PdfPage page = docEvent.GetPage();

		Rectangle pageSize = page.GetPageSize();
		float x = (pageSize.GetLeft() + pageSize.GetRight()) / 2;
		float y = pageSize.GetBottom() + 20;

		Canvas canvas = new Canvas(page, pageSize);
		canvas.SetFont(_font);
		canvas.SetFontSize(10);
		canvas.ShowTextAligned(_footer, x, y, TextAlignment.CENTER);
		canvas.Close();
	}
}