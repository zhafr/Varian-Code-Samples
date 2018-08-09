﻿////////////////////////////////////////////////////////////////////////////////
// Cleanup.cs
//
// Plugin script to remove all courses and plans from the patient currently 
// open in Eclipse. Used to cleanup the results after the demo script has 
// been executed.
//  
// Applies to: ESAPI v13, v13.5, v13.6, and v15.6
//
// Copyright (c) 2018 Varian Medical Systems, Inc.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy 
// of this software and associated documentation files (the "Software"), to deal 
// in the Software without restriction, including without limitation the rights 
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell 
// copies of the Software, and to permit persons to whom the Software is 
// furnished to do so, subject to the following conditions:
//
//  The above copyright notice and this permission notice shall be included in 
//  all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR 
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER 
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, 
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN 
// THE SOFTWARE.
////////////////////////////////////////////////////////////////////////////////

using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Linq;
using VMS.TPS.Common.Model.API;

namespace VMS.TPS
{
    public class Script
    {

      public Script()
      {
      }

      public void Execute(ScriptContext ctx)
      {
        var patient = ctx.Patient;
        patient.BeginModifications();
        if (patient != null)
        {
          if (patient.Courses.Any())
          {
            var courses = patient.Courses.ToList();
            foreach (var course in courses)
            {
              patient.RemoveCourse(course);
            }
          }

          const string PatientStructures = "Prostate";
          var removedStructureIds = new List<string> {"PTV", "CTV+margin"};
          if (patient.StructureSets.Any(set => set.Id == PatientStructures))
          {
            var structureSet = patient.StructureSets.Single(set => set.Id == PatientStructures);
            foreach (var id in removedStructureIds)
            {
              if (structureSet.Structures.Any(st => st.Id == id))
              {
                var removedStructure = structureSet.Structures.Single(st => st.Id == id);
                structureSet.RemoveStructure(removedStructure);
              }
            }  
          }
        }
      }
    }
}
