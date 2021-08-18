using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using RatingAdjustment.Services;
using BreadmakerReport.Models;
using System.Collections.Generic;


namespace BreadmakerReport
{
    class Program
    {
        static string dbfile = @".\data\breadmakers.db";
        static RatingAdjustmentService ratingAdjustmentService = new RatingAdjustmentService();

        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Bread World");
            var BreadmakerDb = new BreadMakerSqliteContext(dbfile);
            var BMList = BreadmakerDb.Breadmakers
                .Include(i => i.Reviews)
                .AsEnumerable()
                .Select(sel => new {
                    Reviews = sel.Reviews.Count,
                    Average = Math.Round(sel.Reviews.Average(s => s.stars), 2),
                    Adjust = Math.Round(ratingAdjustmentService.Adjust(sel.Reviews.Average(s => s.stars), sel.Reviews.Count()), 2),
                    sel.title
                })
                .OrderByDescending(bm => bm.Adjust)
                .ToList();

            Console.WriteLine("[#]  Reviews Average  Adjust    Description");
            for (var j = 0; j < 3; j++)
            {
                var i = BMList[j];
                // TODO: add output
                Console.WriteLine("\n[{0}] {1} {2} {3} {4}", j + 1, i.Reviews, i.Average, i.Adjust, i.title);
            }
        }
    }
}
