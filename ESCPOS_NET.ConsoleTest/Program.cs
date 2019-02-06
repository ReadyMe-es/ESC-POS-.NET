﻿using ESCPOS_NET.Emitters;
using System;
using System.Threading;

namespace ESCPOS_NET.ConsoleTest
{
    class Program
    {
        private static SerialPrinter sp;
        private static ICommandEmitter e;


        static void Main(string[] args)
        {
            sp = new SerialPrinter("COM20", 115200);
            e = new EPSON_TM_T20II();
            while (true)
            { 
                Setup();
                //TestStyles();
                //TestLineFeeds();
                //TestCutter();
                //TestMultiLineWrite();
                //TestBHReceipt();
                TestBarcodeStyles();
                var x = Console.ReadKey();
                //TestHEBReceipt();
                // TODO: write a sanitation check.
                // TODO: make DPI to inch convesion function
                // TODO: full cuts and reverse feeding not implemented on epson...  should throw exception?
                // TODO: make an input loop that lets you execute each test separately.
                // TODO: also make an automatic runner that runs all tests (command line).
                //Thread.Sleep(1000);
            }
        }

        static void Setup()
        {
            sp.Write(e.Initialize);
            sp.Write(e.Enable);
        }

        static void TestMultiLineWrite()
        {
            sp.Write(e.PrintLines("This is a test\r\nOf multiline\rprinting with different\n line separators.\n\n"));
        }

        static void TestStyles()
        {
            sp.Write(e.SetStyles(PrintStyle.None));
            sp.Write(e.PrintLines("Default: The quick brown fox jumped over the lazy dogs.\n"));
            sp.Write(e.SetStyles(PrintStyle.FontB));
            sp.Write(e.PrintLines("Font B: The quick brown fox jumped over the lazy dogs.\n"));
            sp.Write(e.SetStyles(PrintStyle.Bold));
            sp.Write(e.PrintLines("Bold: The quick brown fox jumped over the lazy dogs.\n"));
            sp.Write(e.SetStyles(PrintStyle.Underline));
            sp.Write(e.PrintLines("Underline: The quick brown fox jumped over the lazy dogs.\n"));
            sp.Write(e.SetStyles(PrintStyle.DoubleWidth));
            sp.Write(e.PrintLines("DoubleWidth: The quick brown fox jumped over the lazy dogs.\n"));
            sp.Write(e.SetStyles(PrintStyle.DoubleHeight));
            sp.Write(e.PrintLines("DoubleHeight: The quick brown fox jumped over the lazy dogs.\n"));
            sp.Write(e.SetStyles(PrintStyle.FontB | PrintStyle.DoubleHeight | PrintStyle.DoubleWidth | PrintStyle.Underline | PrintStyle.Bold));
            sp.Write(e.PrintLines("All Styles: The quick brown fox jumped over the lazy dogs.\n"));
            sp.Write(e.SetStyles(PrintStyle.None));
        }
        static void TestLineFeeds()
        {
            sp.Write(e.PrintLine("Feeding 1000 dots."));
            sp.Write(e.FeedDots(1000));
            sp.Write(e.PrintLine("Feeding 3 lines."));
            sp.Write(e.FeedLines(3));
            sp.Write(e.PrintLine("Done Feeding."));
            sp.Write(e.PrintLine("Reverse Feeding 6 lines."));
            sp.Write(e.FeedLinesReverse(6));
            sp.Write(e.PrintLine("Done Reverse Feeding."));
        }

        static void TestCutter()
        {
            sp.Write(e.PrintLine("Performing Full Cut (no feed)."));
            sp.Write(e.FullCut);
            sp.Write(e.PrintLine("Performing Partial Cut (no feed)."));
            sp.Write(e.PartialCut);
            sp.Write(e.PrintLine("Performing Full Cut (1000 dot feed)."));
            sp.Write(e.FullCutAfterFeed(1000));
            sp.Write(e.PrintLine("Performing Partial Cut (1000 dot feed)."));
            sp.Write(e.PartialCutAfterFeed(1000));
        }

        static void TestBarcodeStyles()
        {
            sp.Write(
                //TODO: test all wqidths and put bar in front in label
                e.PrintLine("Starting Barcode Test..."),
                e.PrintLine("Narrow Width:"),
                e.SetBarcodeHeightInDots(300),
                e.SetBarWidth(BarWidth.Thinnest),
                e.PrintBarcode(BarcodeType.UPC_A, "012345678905"),

                e.PrintLine("Wide Width:"),
                e.SetBarWidth(BarWidth.Thickest),
                e.PrintBarcode(BarcodeType.UPC_A, "012345678905"),

                e.PrintLine("Short:"),
                e.SetBarcodeHeightInDots(50),
                e.SetBarWidth(BarWidth.Regular),
                e.PrintBarcode(BarcodeType.UPC_A, "012345678905"),

                e.PrintLine("Tall:"),
                e.SetBarcodeHeightInDots(255),
                e.PrintBarcode(BarcodeType.UPC_A, "012345678905"),

                e.PrintLine("Label Above:"),
                e.SetBarcodeHeightInDots(50),
                e.SetBarLabelPosition(BarLabelPrintPosition.Above),
                e.PrintBarcode(BarcodeType.UPC_A, "012345678905"),

                e.PrintLine("Label Above and Below:"),
                e.SetBarLabelPosition(BarLabelPrintPosition.Both),
                e.PrintBarcode(BarcodeType.UPC_A, "012345678905"),

                e.PrintLine("Label Below:"),
                e.SetBarLabelPosition(BarLabelPrintPosition.Below),
                e.PrintBarcode(BarcodeType.UPC_A, "012345678905"),

                e.PrintLine("Font B Label Below:"),
                e.SetBarLabelFontB(true),
                e.PrintBarcode(BarcodeType.UPC_A, "012345678905"),

                e.FullCutAfterFeed(1000)
            );



        }
        static void TestBarcodeTypes()
        {
            sp.Write(
              e.PrintLine("UPC_A: 012345678905 "),
                e.SetBarLabelFontB(false),
                e.SetBarLabelPosition(BarLabelPrintPosition.Below),
                e.PrintBarcode(BarcodeType.UPC_A, "012345678905")
                );
            /*
             * 
                e.PrintBarcode(BarcodeType.CODE128, "10945500020119184400014"),
                /*
            e.PrintBarcode(BarcodeType., "041220096138"),
             *         UPC_A                       = 0x41,
        UPC_E                       = 0x42,
        JAN13_EAN13                 = 0x43,
        JAN8_EAN8                   = 0x44,
        CODE39                      = 0x45,
        ITF                         = 0x46,
        CODABAR_NW_7                = 0x47,
        CODE93                      = 0x48,
        CODE128                     = 0x49,
        GS1_128                     = 0x4A,
        GS1_DATABAR_OMNIDIRECTIONAL = 0x4B,
        GS1_DATABAR_TRUNCATED       = 0x4C,
        GS1_DATABAR_LIMITED         = 0x4D,
        GS1_DATABAR_EXPANDED        = 0x4E


            */

        }


        static void TestHEBReceipt()
        {
            sp.Write(
                //e.FeedDots(2000),
                // TODO: logo
                e.CenterAlign,
                //e.PrintLine("BHONEYWELLv"),
                //e.SetBarcodeHeightInDots(360),
                //e.SetBarWidth(BarWidth.Regular),
                //e.SetBarLabelPosition(BarLabelPrintPosition.None),
                e.PrintBarcode(BarcodeType.JAN13_EAN13, "041220096138"),
                /*
                e.PrintLine(""),
                e.PrintLine("B&H PHOTO & VIDEO"),
                e.PrintLine("420 NINTH AVE."),
                e.PrintLine("NEW YORK, NY 10001"),
                e.PrintLine("(212) 502-6380 - (800)947-9975"),
                e.SetStyles(PrintStyle.Underline),
                e.PrintLine("www.bhphotovideo.com"),
                e.SetStyles(PrintStyle.None),
                e.PrintLine(""),
                e.LeftAlign,
                e.PrintLine("Order: 123456789        Date: 02/01/19"),
                e.PrintLine(""),
                e.PrintLine(""),
                e.SetStyles(PrintStyle.FontB),
                e.PrintLine("1   TRITON LOW-NOISE IN-LINE MICROPHONE PREAMP"),
                e.PrintLine("    TRFETHEAD/FETHEAD                        89.95         89.95"),
                e.PrintLine("----------------------------------------------------------------"),
                e.RightAlign,
                e.PrintLine("SUBTOTAL         89.95"),
                e.PrintLine("Total Order:         89.95"),
                e.PrintLine("Total Payment:              "),
                e.PrintLine(""),
                e.LeftAlign,
                e.SetStyles(PrintStyle.Bold | PrintStyle.FontB),
                e.PrintLine("SOLD TO:                        SHIP TO:"),
                e.SetStyles(PrintStyle.FontB),
                e.PrintLine("  LUKE PAIREEPINART               LUKE PAIREEPINART"),
                e.PrintLine("  123 FAKE ST.                    123 FAKE ST."),
                e.PrintLine("  DECATUR, IL 12345               DECATUR, IL 12345"),
                e.PrintLine("  (123)456-7890                   (123)456-7890"),
                e.PrintLine("  CUST: 87654321"),*/
            e.FullCutAfterFeed(1000)
            );
        }


        static void TestBHReceipt()
        {
            sp.Write(
                e.FeedDots(2000),
                e.CenterAlign,
                e.PrintLine("3"),
                e.SetBarcodeHeightInDots(360),
                e.SetBarWidth(BarWidth.Regular),
                e.SetBarLabelPosition(BarLabelPrintPosition.None),
                e.PrintBarcode(BarcodeType.ITF, "0123456789"),
                e.PrintLine(""),
                e.PrintLine("B&H PHOTO & VIDEO"),
                e.PrintLine("420 NINTH AVE."),
                e.PrintLine("NEW YORK, NY 10001"),
                e.PrintLine("(212) 502-6380 - (800)947-9975"),
                e.SetStyles(PrintStyle.Underline),
                e.PrintLine("www.bhphotovideo.com"),
                e.SetStyles(PrintStyle.None),
                e.PrintLine(""),
                e.LeftAlign,
                e.PrintLine("Order: 123456789        Date: 02/01/19"),
                e.PrintLine(""),
                e.PrintLine(""),
                e.SetStyles(PrintStyle.FontB),
                e.PrintLine("1   TRITON LOW-NOISE IN-LINE MICROPHONE PREAMP"),
                e.PrintLine("    TRFETHEAD/FETHEAD                        89.95         89.95"),
                e.PrintLine("----------------------------------------------------------------"),
                e.RightAlign,
                e.PrintLine("SUBTOTAL         89.95"),
                e.PrintLine("Total Order:         89.95"),
                e.PrintLine("Total Payment:              "),
                e.PrintLine(""),
                e.LeftAlign,
                e.SetStyles(PrintStyle.Bold | PrintStyle.FontB),
                e.PrintLine("SOLD TO:                        SHIP TO:"),
                e.SetStyles(PrintStyle.FontB),
                e.PrintLine("  LUKE PAIREEPINART               LUKE PAIREEPINART"),
                e.PrintLine("  123 FAKE ST.                    123 FAKE ST."),
                e.PrintLine("  DECATUR, IL 12345               DECATUR, IL 12345"),
                e.PrintLine("  (123)456-7890                   (123)456-7890"),
                e.PrintLine("  CUST: 87654321"),
                e.FullCutAfterFeed(1000)
            );
        }
    }
}
