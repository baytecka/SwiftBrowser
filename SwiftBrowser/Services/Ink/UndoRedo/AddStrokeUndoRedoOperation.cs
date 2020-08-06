﻿using System.Collections.Generic;
using System.Linq;
using Windows.UI.Input.Inking;
using flowpad.EventHandlers.Ink;
using flowpad.Services.Ink;

namespace Flowpad.Services.Ink.UndoRedo
{
    public class AddStrokeUndoRedoOperation : IUndoRedoOperation
    {
        private readonly List<InkStroke> _strokes;
        private readonly InkStrokesService _strokeService;

        public AddStrokeUndoRedoOperation(IEnumerable<InkStroke> strokes, InkStrokesService strokeService)
        {
            _strokes = new List<InkStroke>(strokes);
            _strokeService = strokeService;

            _strokeService.AddStrokeEvent += StrokeService_AddStrokeEvent;
        }

        public void ExecuteUndo()
        {
            _strokes.ForEach(s => _strokeService.RemoveStroke(s));
        }

        public void ExecuteRedo()
        {
            _strokes.ToList().ForEach(s => _strokeService.AddStroke(s));
        }

        private void StrokeService_AddStrokeEvent(object sender, AddStrokeEventArgs e)
        {
            if (e.NewStroke == null) return;

            var removedStrokes = _strokes.RemoveAll(s => s.Id == e.OldStroke?.Id);
            if (removedStrokes > 0) _strokes.Add(e.NewStroke);
        }
    }
}
