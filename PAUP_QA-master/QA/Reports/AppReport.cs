using iTextSharp.text;
using iTextSharp.text.pdf;
using QA.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace QA.Reports
{
    public class AppReport
    {
		BazaDbContext bazaPodataka = new BazaDbContext();
		public byte[] Podatci { get; private set; }

		public void pdfSvaPitanja(MixModel podaci)
		{
			BaseFont bfontHeader = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1250, false);
			BaseFont bfontText = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1250, true);
			BaseFont bfontFooter = BaseFont.CreateFont(BaseFont.TIMES_ITALIC, BaseFont.CP1250, false);

			Font headerBold = new Font(bfontText, 16, Font.BOLD, BaseColor.DARK_GRAY);
			Font naslov = new Font(bfontText, 12, Font.BOLDITALIC, BaseColor.DARK_GRAY);
			Font tekst = new Font(bfontText, 12, Font.NORMAL, BaseColor.BLACK);

			using (MemoryStream memo = new MemoryStream())
			{
				using (Document pdfDokument = new Document(PageSize.A4, 50, 50, 20, 50))
				{
					PdfWriter.GetInstance(pdfDokument, memo).CloseStream = false;
					pdfDokument.Open();

					Paragraph q = new Paragraph("QBox", headerBold);
					q.Alignment = Element.ALIGN_CENTER;
					q.SpacingBefore = 10;
					q.SpacingAfter = 10;
					pdfDokument.Add(q);

					Paragraph p = new Paragraph("Popis pitanja", naslov);
					p.Alignment = Element.ALIGN_CENTER;
					p.SpacingBefore = 20;
					p.SpacingAfter = 20;
					pdfDokument.Add(p);


					BaseColor colorheader = BaseColor.PINK;

					PdfPTable t = new PdfPTable(6);
					t.WidthPercentage = 100;
					t.SetWidths(new float[] { 1, 4, 3, 2, 3, 1 });

					t.AddCell(vratiCeliju("R.br", tekst, colorheader, true));
					t.AddCell(vratiCeliju("Pitanje", tekst, colorheader, true));
					t.AddCell(vratiCeliju("Korisničko ime", tekst, colorheader, true));
					t.AddCell(vratiCeliju("Objavljeno", tekst, colorheader, true));
					t.AddCell(vratiCeliju("Kategorija", tekst, colorheader, true));
					t.AddCell(vratiCeliju("Odg.", tekst, colorheader, true));

					int i = 1;
					foreach (var pit in podaci.Pitanja.ToList())
					{
						t.AddCell(vratiCeliju(i.ToString() + ".", tekst, BaseColor.WHITE, false));
						t.AddCell(vratiCeliju(pit.pitanjeTekst, tekst, BaseColor.WHITE, false));
						t.AddCell(vratiCeliju(pit.korisnickoIme.korisnicko_ime, tekst, BaseColor.WHITE, false));
						t.AddCell(vratiCeliju(pit.datumObjave.Day.ToString() + "." + pit.datumObjave.Month.ToString() + "." + pit.datumObjave.Year.ToString(), tekst, BaseColor.WHITE, false));
						t.AddCell(vratiCeliju(pit.kategorijaId.kategorija, tekst, BaseColor.WHITE, false));
						t.AddCell(vratiCeliju(podaci.Odgovori.Where(x=>x.pitanje_id==pit.id).Count().ToString(), tekst, BaseColor.WHITE, false));
						i++;
					}

					pdfDokument.Add(t);


					p = new Paragraph("Čakovec, " + DateTime.Now.ToString("dd.MM.yyyy"), tekst);
					p.Alignment = Element.ALIGN_RIGHT;
					p.SpacingBefore = 30;

					pdfDokument.Add(p);

				}

				Podatci = memo.ToArray();

				using (var reader = new PdfReader(Podatci))
				{
					using (var ms = new MemoryStream())
					{
						using (var stamper = new PdfStamper(reader, ms))
						{
							int pageCount = reader.NumberOfPages;
							for (int i = 1; i <= pageCount; i++)
							{
								Rectangle pageSize = reader.GetPageSize(i);
								PdfContentByte canvas = stamper.GetOverContent(i);

								canvas.BeginText();
								canvas.SetFontAndSize(bfontFooter, 10);

								canvas.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, $"Stranica {i} / {pageCount}", pageSize.Right - 50, 30, 0);
								canvas.EndText();
							}
						}
						Podatci = ms.ToArray();
					}
				}
			}

		}

		public void pdfSviKorisnici(List<Korisnik> korisnici)
		{
			BaseFont bfontHeader = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1250, false);
			BaseFont bfontText = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1250, true);
			BaseFont bfontFooter = BaseFont.CreateFont(BaseFont.TIMES_ITALIC, BaseFont.CP1250, false);

			Font headerBold = new Font(bfontText, 16, Font.BOLD, BaseColor.DARK_GRAY);
			Font naslov = new Font(bfontText, 12, Font.BOLDITALIC, BaseColor.DARK_GRAY);
			Font tekst = new Font(bfontText, 12, Font.NORMAL, BaseColor.BLACK);

			MixModel model = new MixModel();
			model.Pitanja = bazaPodataka.PopisPitanja.ToList().OrderByDescending(x => x.datumObjave).ThenBy(x => x.kategorijaId.kategorija);
			model.Odgovori = bazaPodataka.PopisOdgovora.ToList().OrderByDescending(x => x.datumObjave).ThenBy(x => x.Pit.pitanjeTekst);

			using (MemoryStream memo = new MemoryStream())
			{
				using (Document pdfDokument = new Document(PageSize.A4, 50, 50, 20, 50))
				{
					PdfWriter.GetInstance(pdfDokument, memo).CloseStream = false;
					pdfDokument.Open();

					Paragraph q = new Paragraph("QBox", headerBold);
					q.Alignment = Element.ALIGN_CENTER;
					q.SpacingBefore = 10;
					q.SpacingAfter = 10;
					pdfDokument.Add(q);

					Paragraph p = new Paragraph("Popis korisnika", naslov);
					p.Alignment = Element.ALIGN_CENTER;
					p.SpacingBefore = 20;
					p.SpacingAfter = 20;
					pdfDokument.Add(p);


					BaseColor colorheader = BaseColor.PINK;

					PdfPTable t = new PdfPTable(5);
					t.WidthPercentage = 100;
					t.SetWidths(new float[] { 2, 4, 2, 2, 2 });

					t.AddCell(vratiCeliju("R.br", tekst, colorheader, true));
					t.AddCell(vratiCeliju("Korisničko ime", tekst, colorheader, true));
					t.AddCell(vratiCeliju("Ovlast", tekst, colorheader, true));
					t.AddCell(vratiCeliju("Pitanja", tekst, colorheader, true));
					t.AddCell(vratiCeliju("Odgovora", tekst, colorheader, true));

					int i = 1;
					foreach (var kor in korisnici)
					{
						t.AddCell(vratiCeliju(i.ToString() + ".", tekst, BaseColor.WHITE, false));
						t.AddCell(vratiCeliju(kor.korisnicko_ime, tekst, BaseColor.WHITE, false));
						t.AddCell(vratiCeliju(kor.ovlast_sifra, tekst, BaseColor.WHITE, false));
						t.AddCell(vratiCeliju(model.Pitanja.Where(x=>x.korisnicko_ime==kor.id).Count().ToString(), tekst, BaseColor.WHITE, false));
						t.AddCell(vratiCeliju(model.Odgovori.Where(x => x.korisnicko_ime == kor.id).Count().ToString(), tekst, BaseColor.WHITE, false));
						i++;
					}

					pdfDokument.Add(t);


					p = new Paragraph("Čakovec, " + DateTime.Now.ToString("dd.MM.yyyy"), tekst);
					p.Alignment = Element.ALIGN_RIGHT;
					p.SpacingBefore = 30;

					pdfDokument.Add(p);

				}

				Podatci = memo.ToArray();

				using (var reader = new PdfReader(Podatci))
				{
					using (var ms = new MemoryStream())
					{
						using (var stamper = new PdfStamper(reader, ms))
						{
							int pageCount = reader.NumberOfPages;
							for (int i = 1; i <= pageCount; i++)
							{
								Rectangle pageSize = reader.GetPageSize(i);
								PdfContentByte canvas = stamper.GetOverContent(i);

								canvas.BeginText();
								canvas.SetFontAndSize(bfontFooter, 10);

								canvas.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, $"Stranica {i} / {pageCount}", pageSize.Right - 50, 30, 0);
								canvas.EndText();
							}
						}
						Podatci = ms.ToArray();
					}
				}
			}
		}

		public void pdfKorisnikPitanja(Korisnik korisnik)
		{
			BaseFont bfontHeader = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1250, false);
			BaseFont bfontText = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1250, true);
			BaseFont bfontFooter = BaseFont.CreateFont(BaseFont.TIMES_ITALIC, BaseFont.CP1250, false);

			Font headerBold = new Font(bfontText, 16, Font.BOLD, BaseColor.DARK_GRAY);
			Font naslov = new Font(bfontText, 12, Font.BOLDITALIC, BaseColor.DARK_GRAY);
			Font tekst = new Font(bfontText, 12, Font.NORMAL, BaseColor.BLACK);

			MixModel model = new MixModel();
			model.Pitanja = bazaPodataka.PopisPitanja.Where(x=>x.korisnicko_ime==korisnik.id).ToList().OrderByDescending(x=>x.datumObjave).ThenByDescending(x=>x.kategorijaId.kategorija);
			model.Odgovori = bazaPodataka.PopisOdgovora.ToList().OrderByDescending(x => x.datumObjave).ThenBy(x => x.Pit.pitanjeTekst);

			using (MemoryStream memo = new MemoryStream())
			{
				using (Document pdfDokument = new Document(PageSize.A4, 50, 50, 20, 50))
				{
					PdfWriter.GetInstance(pdfDokument, memo).CloseStream = false;
					pdfDokument.Open();

					Paragraph q = new Paragraph("QBox", headerBold);
					q.Alignment = Element.ALIGN_CENTER;
					q.SpacingBefore = 10;
					q.SpacingAfter = 10;
					pdfDokument.Add(q);

					Paragraph p = new Paragraph(korisnik.korisnicko_ime, naslov);
					p.Alignment = Element.ALIGN_CENTER;
					p.SpacingBefore = 20;
					p.SpacingAfter = 20;
					pdfDokument.Add(p);

					BaseColor colorheader = BaseColor.PINK;

					PdfPTable t = new PdfPTable(4);
					t.WidthPercentage = 100;
					t.SetWidths(new float[] { 2, 4, 2, 2 });

					t.AddCell(vratiCeliju("R.br", tekst, colorheader, true));
					t.AddCell(vratiCeliju("Pitanje", tekst, colorheader, true));
					t.AddCell(vratiCeliju("Objavljeno", tekst, colorheader, true));
					t.AddCell(vratiCeliju("Odgovora", tekst, colorheader, true));

					int i = 1;
					foreach (var pit in model.Pitanja)
					{
						t.AddCell(vratiCeliju(i.ToString() + ".", tekst, BaseColor.WHITE, false));
						t.AddCell(vratiCeliju(pit.pitanjeTekst, tekst, BaseColor.WHITE, false));
						t.AddCell(vratiCeliju(pit.datumObjave.Day.ToString() + "." + pit.datumObjave.Month.ToString() + "." + pit.datumObjave.Year.ToString(), tekst, BaseColor.WHITE, false));
						t.AddCell(vratiCeliju(model.Odgovori.Where(x => x.pitanje_id == pit.id).Count().ToString(), tekst, BaseColor.WHITE, false));
						i++;
					}

					pdfDokument.Add(t);


					p = new Paragraph("Čakovec, " + DateTime.Now.ToString("dd.MM.yyyy"), tekst);
					p.Alignment = Element.ALIGN_RIGHT;
					p.SpacingBefore = 30;

					pdfDokument.Add(p);

				}

				Podatci = memo.ToArray();

				using (var reader = new PdfReader(Podatci))
				{
					using (var ms = new MemoryStream())
					{
						using (var stamper = new PdfStamper(reader, ms))
						{
							int pageCount = reader.NumberOfPages;
							for (int i = 1; i <= pageCount; i++)
							{
								Rectangle pageSize = reader.GetPageSize(i);
								PdfContentByte canvas = stamper.GetOverContent(i);

								canvas.BeginText();
								canvas.SetFontAndSize(bfontFooter, 10);

								canvas.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, $"Stranica {i} / {pageCount}", pageSize.Right - 50, 30, 0);
								canvas.EndText();
							}
						}
						Podatci = ms.ToArray();
					}
				}
			}
		}

		public void pdfKorisnikOdgovori(Korisnik korisnik)
		{
			BaseFont bfontHeader = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1250, false);
			BaseFont bfontText = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1250, true);
			BaseFont bfontFooter = BaseFont.CreateFont(BaseFont.TIMES_ITALIC, BaseFont.CP1250, false);

			Font headerBold = new Font(bfontText, 16, Font.BOLD, BaseColor.DARK_GRAY);
			Font naslov = new Font(bfontText, 12, Font.BOLDITALIC, BaseColor.DARK_GRAY);
			Font tekst = new Font(bfontText, 12, Font.NORMAL, BaseColor.BLACK);

			MixModel model = new MixModel();
			model.Pitanja = bazaPodataka.PopisPitanja.ToList();
			model.Odgovori = bazaPodataka.PopisOdgovora.Where(x=>x.korisnicko_ime==korisnik.id).ToList().OrderByDescending(x=>x.datumObjave).ThenBy(x=>x.najdraze);

			using (MemoryStream memo = new MemoryStream())
			{
				using (Document pdfDokument = new Document(PageSize.A4, 50, 50, 20, 50))
				{
					PdfWriter.GetInstance(pdfDokument, memo).CloseStream = false;
					pdfDokument.Open();

					Paragraph q = new Paragraph("QBox", headerBold);
					q.Alignment = Element.ALIGN_CENTER;
					q.SpacingBefore = 10;
					q.SpacingAfter = 10;
					pdfDokument.Add(q);

					Paragraph p = new Paragraph(korisnik.korisnicko_ime, naslov);
					p.Alignment = Element.ALIGN_CENTER;
					p.SpacingBefore = 20;
					p.SpacingAfter = 20;
					pdfDokument.Add(p);

					BaseColor colorheader = BaseColor.PINK;

					PdfPTable t = new PdfPTable(5);
					t.WidthPercentage = 100;
					t.SetWidths(new float[] { 2, 4, 4, 2, 2 });

					t.AddCell(vratiCeliju("R.br", tekst, colorheader, true));
					t.AddCell(vratiCeliju("Pitanje", tekst, colorheader, true));
					t.AddCell(vratiCeliju("Odgovor", tekst, colorheader, true));
					t.AddCell(vratiCeliju("Objavljeno", tekst, colorheader, true));
					t.AddCell(vratiCeliju("Najdraži", tekst, colorheader, true));

					int i = 1;
					foreach (var odg in model.Odgovori)
					{
						t.AddCell(vratiCeliju(i.ToString() + ".", tekst, BaseColor.WHITE, false));
						t.AddCell(vratiCeliju(odg.Pit.pitanjeTekst, tekst, BaseColor.WHITE, false));
						t.AddCell(vratiCeliju(odg.odgovor, tekst, BaseColor.WHITE, false));
						t.AddCell(vratiCeliju(odg.datumObjave.Day.ToString() + "." + odg.datumObjave.Month.ToString() + "." + odg.datumObjave.Year.ToString(), tekst, BaseColor.WHITE, false));
						t.AddCell(vratiCeliju(odg.najdraze ? "DA" : "NE", tekst, BaseColor.WHITE, false));
						i++;
					}

					pdfDokument.Add(t);


					p = new Paragraph("Čakovec, " + DateTime.Now.ToString("dd.MM.yyyy"), tekst);
					p.Alignment = Element.ALIGN_RIGHT;
					p.SpacingBefore = 30;

					pdfDokument.Add(p);

				}

				Podatci = memo.ToArray();

				using (var reader = new PdfReader(Podatci))
				{
					using (var ms = new MemoryStream())
					{
						using (var stamper = new PdfStamper(reader, ms))
						{
							int pageCount = reader.NumberOfPages;
							for (int i = 1; i <= pageCount; i++)
							{
								Rectangle pageSize = reader.GetPageSize(i);
								PdfContentByte canvas = stamper.GetOverContent(i);

								canvas.BeginText();
								canvas.SetFontAndSize(bfontFooter, 10);

								canvas.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, $"Stranica {i} / {pageCount}", pageSize.Right - 50, 30, 0);
								canvas.EndText();
							}
						}
						Podatci = ms.ToArray();
					}
				}
			}
		}

		private PdfPCell vratiCeliju(string labela, Font font, BaseColor boja, bool wrap)
		{
			PdfPCell c1 = new PdfPCell(new Phrase(labela, font));
			c1.BackgroundColor = boja;
			c1.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
			c1.Padding = 5;
			c1.NoWrap = wrap;

			return c1;
		}
	}
}